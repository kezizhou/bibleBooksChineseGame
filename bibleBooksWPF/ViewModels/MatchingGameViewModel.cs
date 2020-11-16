using BibleBooksWPF.Helpers;
using BibleBooksWPF.UserControls;
using BibleBooksWPF.Views;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

namespace BibleBooksWPF.ViewModels {
	public class MatchingGameViewModel : INotifyPropertyChanged {

		public DispatcherTimer timer1 = new DispatcherTimer();
		public Stopwatch stopwatch = new Stopwatch();
		public RelayCommand pauseButtonCommand { get; private set; }

		public MatchingGameViewModel() {
			try {
				pauseButtonCommand = new RelayCommand(Pause);

				// Reset timer
				timer1.Tick += new EventHandler(timer1_Tick);
				timer1.Interval = new TimeSpan(0, 0, 0, 1);
				stopwatch.Reset();

				// Start timer
				stopwatch.Start();
				timer1.Start();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private int intNumberAttempted = 0;
		public int propNumberAttempted {
			get { 
				return intNumberAttempted;
			}
			set { 
				intNumberAttempted = value;
				NotifyPropertyChanged();
				propPercentageCorrect = (double)propNumberCorrect / propNumberAttempted;
			}
		}

		private int intNumberCorrect = 0;
		public int propNumberCorrect {
			get { 
				return intNumberCorrect;
			}
			set { 
				intNumberCorrect = value;
				NotifyPropertyChanged();
			}
		}

		private int intCurrentPoints = 0;
		public int propCurrentPoints {
			get { 
				return intCurrentPoints;
			}
			set { 
				intCurrentPoints = value;
				NotifyPropertyChanged();
			}
		}

		private long lngTotalPoints = Properties.Settings.Default.lngTotalPoints;
		public long propTotalPoints {
			get { 
				return lngTotalPoints; 
			}
			set {
				if (value == lngTotalPoints) return;
				lngTotalPoints = value;
				Properties.Settings.Default.lngTotalPoints = value;
				Properties.Settings.Default.Save();
				NotifyPropertyChanged();
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

		private double dblPercentageCorrect = 0;
		public double propPercentageCorrect {
			get { 
				return dblPercentageCorrect; 
			}
			set {
				dblPercentageCorrect = value;
				NotifyPropertyChanged();
			}
		}

		internal void AddCorrectAttempt() {
			propNumberCorrect += 1;
			propCurrentPoints += 1;
			propTotalPoints += 1;
			propNumberAttempted += 1;
		}

		internal void AddIncorrectAttempt() {
			propCurrentPoints -= 1;
			propTotalPoints -= 1;
			propNumberAttempted += 1;
		}

		private void timer1_Tick(object sender, EventArgs e) {
			try {
				if (stopwatch.IsRunning) {
					propTimeElapsed = stopwatch.Elapsed;
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

		internal Tuple<bool, bool> checkTouchingLabelsCorrect(Label lbl, Label lblCh, Rect rctChLbl, string[] astrBooks, string[] astrChBooks) {
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Turn English label into a rectangle
			Rect rctLbl = new Rect();
			rctLbl.Location = lbl.PointToScreen(new Point(0, 0));
			rctLbl.Height = lbl.ActualHeight;
			rctLbl.Width = lbl.ActualWidth;

			// Only check English labels that are touching the Chinese label
			if (rctChLbl.IntersectsWith(rctLbl) && lbl.IsEnabled) {
				int intChLabelIndex = Array.IndexOf(astrChBooks, lblCh.Name);
				blnAttemptedMatch = true;

				// If the correct English label has been matched
				if (lbl.Name == astrBooks[intChLabelIndex]) {
					// Mark boolean flag true first
					// Override the false in case it is touching 2 English labels at once
					blnCorrect = true;

					// Move correct label on top of English
					lblCh.RenderTransform = new TranslateTransform();
					Grid.SetRow(lblCh, Grid.GetRow(lbl));
					Grid.SetColumn(lblCh, Grid.GetColumn(lbl));
					lblCh.IsEnabled = false;
					lbl.IsEnabled = false;
				}
			}

			return Tuple.Create(blnCorrect, blnAttemptedMatch);
		}

		internal Tuple<bool, bool> checkTouchingLabelsCorrect(BibleBook lbl, BibleBook lblCh, Rect rctChLbl, string[] astrBooks, string[] astrChBooks) {
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Turn English label into a rectangle
			Rect rctLbl = new Rect();
			rctLbl.Location = lbl.PointToScreen(new Point(0, 0));
			rctLbl.Height = lbl.ActualHeight;
			rctLbl.Width = lbl.ActualWidth;

			// Only check English labels that are touching the Chinese label
			if (rctChLbl.IntersectsWith(rctLbl) && lbl.IsEnabled) {
				int intChLabelIndex = Array.IndexOf(astrChBooks, lblCh.Name);
				blnAttemptedMatch = true;

				// If the correct English label has been matched
				if (lbl.Name == astrBooks[intChLabelIndex]) {
					// Mark boolean flag true first
					// Override the false in case it is touching 2 English labels at once
					blnCorrect = true;

					// Move correct label on top of English
					lblCh.RenderTransform = new TranslateTransform();
					Grid.SetRow(lblCh, Grid.GetRow(lbl));
					Grid.SetColumn(lblCh, Grid.GetColumn(lbl));
					lblCh.IsEnabled = false;
					lbl.IsEnabled = false;
				}
			}

			return Tuple.Create(blnCorrect, blnAttemptedMatch);
		}

		internal void completedMatching(int intCurrentPoints, int intNumberCorrect, int intNumberAttempted) {
			stopwatch.Stop();
			CustomMessageBox winMsgBox = new CustomMessageBox();

			// Add the game data to statistics json file
			TimeSpan time = new TimeSpan(stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
			string strRecord = StatisticMethods.AddStatistic("GreekMatch", intCurrentPoints, time);

			string strResponse = "";
			string strTime = String.Format("{0:00}:{1:00}:{2:00}", time.Hours, time.Minutes, time.Seconds);
			if (strRecord != "") {
				// Record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intNumberAttempted) + "\n" +
							"Time Elapsed: " + strTime, "Congratulations!", "congrats", strRecord, winMsgBox);
			} else {
				// No record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intNumberAttempted) + "\n" +
							"Time Elapsed: " + strTime, "Congratulations!", "congrats", winMsgBox);
			}

			switch (strResponse) {
				case "Retry":
					ChangeViewMessage.Navigate("HebrewMatch");
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
}
