using GalaSoft.MvvmLight.CommandWpf;
using System.Diagnostics;
using System.Windows.Threading;
using System.Windows;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows.Data;
using System.Globalization;
using System.Linq;

using BibleBooksWPF.Helpers;
using BibleBooksWPF.Views;
using BibleBooksWPF.Models;

namespace BibleBooksWPF.ViewModels {
	public class DiceBooksGreekViewModel : INotifyPropertyChanged {

		public DispatcherTimer timer1 = new DispatcherTimer();
		public Stopwatch stopwatch = new Stopwatch();

		public DispatcherTimer timerTurn = new DispatcherTimer();
		Stopwatch stopwatchTurn = new Stopwatch();

		public RelayCommand pauseButtonCommand { get; private set; }
		public RelayCommand submitButtonCommand { get; private set; }
		public RelayCommand rollButtonCommand { get; private set; }

		public List<string> lstRemaining = new List<string>(){ "Matthew", "Mark", "Luke", "John", "Acts", "Romans", "1 Corinthians", "2 Corinthians", "Galatians", "Ephesians",
													"Philippians", "Colossians", "1 Thessalonians", "2 Thessalonians", "1 Timothy", "2 Timothy", "Titus",
													"Philemon", "Hebrews", "James", "1 Peter", "2 Peter", "1 John", "2 John", "3 John", "Jude", "Revelation" };
		public List<string> lstChineseBooks = new List<string>() { "马太福音", "马可福音", "路加福音", "约翰福音", "使徒行传", "罗马书", "哥林多前书", "哥林多后书", "加拉太书", "以弗所书", "腓立比书", "歌罗西书", "帖撒罗尼迦前书",
															"帖撒罗尼迦后书", "提摩太前书", "提摩太后书", "提多书", "腓利门书", "希伯来书", "雅各书", "彼得前书", "彼得后书", "约翰一书", "约翰二书", "约翰三书", "犹大书", "启示录" };
		public List<string> lstCompleted = new List<string>();

		public DiceBooksGreekViewModel() {
			pauseButtonCommand = new RelayCommand(Pause);
			submitButtonCommand = new RelayCommand(Submit);
			rollButtonCommand = new RelayCommand(Roll);

			// Reset main timer
			timer1.Tick += new EventHandler(timer1_Tick);
			timer1.Interval = TimeSpan.FromSeconds(1);
			stopwatch.Reset();

			// Reset turn timer
			timerTurn.Tick += new EventHandler(timerTurn_Tick);
			timerTurn.Interval = TimeSpan.FromMilliseconds(1);
			stopwatchTurn.Reset();

			// Start timer
			stopwatch.Start();
			timer1.Start();

			if (Properties.Settings.Default.strLanguage == "zh-CN") {
				lstRemaining = lstChineseBooks;
			}

			foreach (string strBook in lstRemaining) {
				propBibleBooks.Add(new DiceBooksBibleBook(strBook));
			}
		}

		private TimeSpan tsTimeElapsed = new TimeSpan(0, 0, 0);
		public TimeSpan propTimeElapsed {
			get {
				return tsTimeElapsed;
			}
			set {
				if (tsTimeElapsed == value) return;
				tsTimeElapsed = value;
				NotifyPropertyChanged();
			}
		}

		private TimeSpan tsTurnTimeRemaining = new TimeSpan(0, 0, 60);
		public TimeSpan propTurnTimeRemaining {
			get {
				return tsTurnTimeRemaining;
			}
			set {
				if (tsTurnTimeRemaining == value) return;
				tsTurnTimeRemaining = value;
				NotifyPropertyChanged();
			}
		}

		private int intPoints;
		public int propPoints {
			get { 
				return intPoints;
			}
			set { 
				intPoints = value;
				NotifyPropertyChanged();
			}
		}

		private int intRoll;
		public int propRoll {
			get { 
				return intRoll; 
			}
			set {
				intRoll = value;
				NotifyPropertyChanged();
			}
		}

		private bool blnSubmitEnabled = true;
		public bool propSubmitEnabled {
			get {
				return blnSubmitEnabled;
			}
			set {
				blnSubmitEnabled = value;
				NotifyPropertyChanged();
			}
		}


		private ObservableCollectionPropertyNotify<DiceBooksEntry> lstEntries = new ObservableCollectionPropertyNotify<DiceBooksEntry>();
		public ObservableCollectionPropertyNotify<DiceBooksEntry> propEntries {
			get { 
				return lstEntries; 
			}
			set { 
				lstEntries = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollectionPropertyNotify<DiceBooksBibleBook> lstBibleBooks = new ObservableCollectionPropertyNotify<DiceBooksBibleBook>();
		public ObservableCollectionPropertyNotify<DiceBooksBibleBook> propBibleBooks {
			get { 
				return lstBibleBooks; 
			}
			set {
				lstBibleBooks = value; 
				NotifyPropertyChanged();
			}
		}

		private void timer1_Tick(object sender, EventArgs e) {
			if (stopwatch.IsRunning) {
				propTimeElapsed = stopwatch.Elapsed;
			}
		}

		private void timerTurn_Tick(object sender, EventArgs e) {
			try {
				if (stopwatchTurn.IsRunning) {
					propTurnTimeRemaining = stopwatchTurn.Elapsed - TimeSpan.FromSeconds(6 * propRoll);
				}

				if (stopwatchTurn.Elapsed == TimeSpan.Zero) {
					timerTurn.Stop();
					Submit();
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Pause() {
			stopwatch.Stop();
			PauseMenu winPause = new PauseMenu();

			string strResponse = CustomMessageBoxMethods.ShowMessage(winPause);

			switch (strResponse) {
				case "Resume":
					stopwatch.Start();
					break;
				case "Main":
					ChangeViewMessage.Navigate("MainMenu");
					break;
				case "Exit":
					Application.Current.Shutdown();
					break;
				default:
					break;
			}
		}

		private void Submit() {
			try {
				propSubmitEnabled = false;
				stopwatchTurn.Stop();

				for (int i = 0; i < propRoll; i++) {
					propEntries[i].blnSubmitted = true;

					string strBook = propEntries[i].strInput.Trim();

					propEntries[i].blnCorrect = CheckIfCorrect(strBook);
					if (propEntries[i].blnCorrect == true) {
						// Get name of Bible book
						if (Char.IsLetter(strBook[0])) {
							strBook = strBook[0] + strBook.Substring(1).ToLower();
						} else {
							strBook = strBook[0] + " " + strBook[2] + strBook.Substring(3).ToLower();
						}

						// Add to grid
						propBibleBooks.Where(b => b.propBook == strBook).FirstOrDefault().propVisibility = Visibility.Visible;
					}	
				}

				propEntries.Refresh();
				propBibleBooks.Refresh();

				if (lstRemaining.Count == 0) {
					CompletedGame();
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private bool? CheckIfCorrect(string strInput) {
			foreach (string strBook in lstRemaining) {
				if (strBook.ToLower() == strInput.ToLower()) {
					// Correct
					lstCompleted.Add(strBook);
					lstRemaining.Remove(strBook);
					propPoints += 1;

					return true;
				}
			}

			foreach (string strCompleted in lstCompleted) {
				if (strCompleted.ToLower() == strInput.ToLower()) {
					if (lstCompleted.Contains(strCompleted)) {
						// Duplicate
						return null;
					}
				}
			}

			// Book not found
			propPoints -= 1;
			return false;
		}

		private void Roll() {
			try {
				// Clear previous
				propEntries.Clear();
				stopwatchTurn.Reset();
				propSubmitEnabled = true;

				Random random = new Random();
				if (lstRemaining.Count < 6) {
					propRoll = random.Next(1, lstRemaining.Count + 1);
				} else {
					propRoll = random.Next(1, 7);
				}

				for (int i = 1; i <= propRoll; i++) {
					propEntries.Add(new DiceBooksEntry());
				}

				//txt1.Focus();

				stopwatchTurn.Start();
				timerTurn.Start();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void CompletedGame() {
			CustomMessageBox winMsgBox = new CustomMessageBox();
			string strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
								"Points Earned: " + propPoints + "\n" +
								"Time Elapsed: " + String.Format("{0:00}:{1:00}.{2:0000}", propTimeElapsed.Minutes, propTimeElapsed.Seconds, propTimeElapsed.Milliseconds), "Congratulations!", "congrats", winMsgBox);

			switch (strResponse) {
				case "Retry":
					ChangeViewMessage.Navigate("DiceBooksGreek");
					break;
				case "Main":
					ChangeViewMessage.Navigate("MainMenu");
					break;
				case "Exit":
					Application.Current.Shutdown();
					break;
				default:
					break;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class TimesUpConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if ((TimeSpan)value == TimeSpan.Zero) {
				return Visibility.Visible;
			}

			return Visibility.Collapsed;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class EntryImageConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) return null;
			
			if ((bool)value == true) {
				return "pack://application:,,,/BibleBooksWPF;component/Resources/correct.png";
			} else {
				return "pack://application:,,,/BibleBooksWPF;component/Resources/incorrect.png";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class SubmittedEnabledConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if ((bool)value == true) {
				return false;
			}  else {
				return true;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class EntryLblHeightConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return (double)value - 20;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}
}
