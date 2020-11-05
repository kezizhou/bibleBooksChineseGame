using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for DiceBooksGreek.xaml
	/// </summary>
	public partial class DiceBooksGreek : Page {

		List<string> lstBooks = new List<string>(){ "Matthew", "Mark", "Luke", "John", "Acts", "Romans", "1 Corinthians", "2 Corinthians", "Galatians", "Ephesians",
													"Philippians", "Colossians", "1 Thessalonians", "2 Thessalonians", "1 Timothy", "2 Timothy", "Titus",
													"Philemon", "Hebrews", "James", "1 Peter", "2 Peter", "1 John", "2 John", "3 John", "Jude", "Revelation" };
		List<string> lstChineseBooks = new List<string>() { "马太福音", "马可福音", "路加福音", "约翰福音", "使徒行传", "罗马书", "哥林多前书", "哥林多后书", "加拉太书", "以弗所书", "腓立比书", "歌罗西书", "帖撒罗尼迦前书",
															"帖撒罗尼迦后书", "提摩太前书", "提摩太后书", "提多书", "腓利门书", "希伯来书", "雅各书", "彼得前书", "彼得后书", "约翰一书", "约翰二书", "约翰三书", "犹大书", "启示录" };
		List<string> lstCompleted = new List<string>();
		DispatcherTimer timer1 = new DispatcherTimer();
		Stopwatch stopwatch = new Stopwatch();

		Stopwatch stopwatchTurn = new Stopwatch();
		DispatcherTimer timerTurn = new DispatcherTimer();

		int intRoll = 0;
		int intPoints = 0;

		public DiceBooksGreek() {
			InitializeComponent();

			// Reset timer
			timer1.Tick += new EventHandler(timer1_Tick);
			timer1.Interval = new TimeSpan(0, 0, 0, 1);
			stopwatch.Reset();

			timerTurn.Tick += new EventHandler(timerTurn_Tick);
			timerTurn.Interval = new TimeSpan(0, 0, 0, 0, 1);
			stopwatchTurn.Reset();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				lblTimeElapsed.Content = "00:00:00";
				lblPoints.Content = intPoints;

				// Start timer
				stopwatch.Start();
				timer1.Start();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void timer1_Tick(object sender, EventArgs e) {
			try {
				if (stopwatch.IsRunning) {
					TimeSpan ts = stopwatch.Elapsed;
					lblTimeElapsed.Content = String.Format("{0:00}:{1:00}:{2:00}",
						ts.Hours, ts.Minutes, ts.Seconds);
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		private void timerTurn_Tick(object sender, EventArgs e) {
			try {
				if (stopwatchTurn.IsRunning) {
					TimeSpan ts = stopwatchTurn.Elapsed;
					lblTimeElapsed.Content = String.Format("{0:00}:{1:00}:{2:00}", ts.Hours, ts.Minutes, ts.Seconds);

					TimeSpan tsTotalTime = new TimeSpan(0, 0, 6 * intRoll);
					TimeSpan tsTimeLeft = tsTotalTime - ts;
					txbTurnTime.Text = String.Format("{0:00}:{1:000}", tsTimeLeft.Seconds, tsTimeLeft.Milliseconds);

					if (stopwatchTurn.Elapsed >= tsTotalTime) {
						txbTimeUp.Visibility = Visibility.Visible;

						// Hide all textboxes
						//for (int i = 1; i <= intRoll; i++) {
						//	TextBox txt = this.FindName("txt" + i) as TextBox;
						//	txt.Visibility = Visibility.Hidden;
						//}

						stopwatchTurn.Reset();
						txbTurnTime.Text = "00:000";
						RoutedEventArgs re = new RoutedEventArgs();
						btnSubmit_Click(sender, re);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenMainMenu_Click(object sender, RoutedEventArgs e) {
			try {
				MainMenu pMainMenu = new MainMenu();
				NavigationService.Navigate(pMainMenu);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenMatchHebrew_Click(object sender, RoutedEventArgs e) {
			try {
				HebrewMatch pHebrewMatch = new HebrewMatch();
				NavigationService.Navigate(pHebrewMatch);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenReorderHebrew_Click(object sender, RoutedEventArgs e) {
			try {
				HebrewReorder pHebrewReorder = new HebrewReorder();
				NavigationService.Navigate(pHebrewReorder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenMatchGreek_Click(object sender, RoutedEventArgs e) {
			try {
				GreekMatch pGreekMatch = new GreekMatch();
				NavigationService.Navigate(pGreekMatch);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenDiceHebrew_Click(object sender, RoutedEventArgs e) {

		}

		private void imenReorderGreek_Click(object sender, RoutedEventArgs e) {
			try {
				GreekReorder pGreekReorder = new GreekReorder();
				NavigationService.Navigate(pGreekReorder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenStatistics_Click(object sender, RoutedEventArgs e) {
			try {
				StatisticsPage pStatisticsPage = new StatisticsPage();
				NavigationService.Navigate(pStatisticsPage);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenSettings_Click(object sender, RoutedEventArgs e) {
			try {
				Settings pSettings = new Settings();
				NavigationService.Navigate(pSettings);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void imenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void btnPause_Click(object sender, RoutedEventArgs e) {
			try {
				stopwatch.Stop();
				PauseMenu winPause = new PauseMenu();

				string strResponse = CustomMessageBoxMethods.ShowMessage(winPause);

				switch (strResponse) {
					case "Resume":
						stopwatch.Start();
						break;
					case "Main":
						MainMenu pMainMenu = new MainMenu();
						NavigationService.Navigate(pMainMenu);
						break;
					case "Exit":
						Application.Current.Shutdown();
						break;
					default:
						break;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void btnSubmit_Click(object sender, RoutedEventArgs e) {
			try {
				stopwatchTurn.Stop();

				for (int i = 1; i <= intRoll; i++) {
					TextBox txt = this.FindName("txt" + i) as TextBox;
					txt.IsEnabled = false;

					string strBook = txt.Text.Trim();

					string strResult = CheckIfCorrect(strBook);
					if (strResult == "correct" || strResult == "completed") {
						// Correct, show check
						Image img = this.FindName("imgIncorrect" + i) as Image;
						img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/correct.png"));
						img.Visibility = Visibility.Visible;

						// Get name of Bible book
						if (Char.IsLetter(strBook[0])) {
							strBook = strBook[0] + strBook.Substring(1).ToLower();
						} else {
							strBook = strBook[0] + " " + strBook[2] + strBook.Substring(3).ToLower();
						}

						// Add to grid
						Label lbl = this.FindName("lbl" + strBook.Replace(" ", "")) as Label;
						lbl.Visibility = Visibility.Visible;

						if (strResult == "completed") {
							CustomMessageBox winMsgBox = new CustomMessageBox();
							string strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
												"Points Earned: " +  intPoints + "\n" +
												"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", winMsgBox);

							switch (strResponse) {
								case "Retry":
									DiceBooksGreek pDiceBooksGreek = new DiceBooksGreek();
									NavigationService.Navigate(pDiceBooksGreek);
									break;
								case "Main":
									MainMenu pMainMenu = new MainMenu();
									NavigationService.Navigate(pMainMenu);
									break;
								case "Exit":
									Application.Current.Shutdown();
									break;
								default:
									break;
							}
						}
					} else if (strResult == "duplicate") {

					} else {
						// Incorrect, show x
						Image img = this.FindName("imgIncorrect" + i) as Image;
						img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/incorrect.png"));
						img.Visibility = Visibility.Visible;
					}

					stopwatchTurn.Reset();
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private string CheckIfCorrect(string strInput) {
			foreach (string strBook in lstBooks) {
				if (strBook.ToLower() == strInput.ToLower()) {
					// Correct
					lstBooks.Remove(strBook);
					lstCompleted.Add(strBook);
					intPoints += 1;
					lblPoints.Content = intPoints;

					if (lstBooks.Count == 0) {
						// Completed
						return "completed";
					}

					return "correct";
				}
			}

			foreach (string strBook in lstCompleted) {
				if (strBook.ToLower() == strInput.ToLower()) {
					// Duplicate
					return "duplicate";
				}
			}

			// Book not found
			intPoints -= 1;
			lblPoints.Content = intPoints;
			return "incorrect";
		}

		private void btnRoll_Click(object sender, RoutedEventArgs e) {
			try {
				// Clear previous
				for (int i = 1; i <= intRoll; i++) {
					TextBox txt = this.FindName("txt" + i) as TextBox;
					txt.Clear();
					txt.IsEnabled = true;

					Image img = this.FindName("imgIncorrect" + i) as Image;
					img.Visibility = Visibility.Collapsed;
				}
				txbTimeUp.Visibility = Visibility.Collapsed;

				Random random = new Random();
				if (lstBooks.Count < 6) {
					intRoll = random.Next(1, lstBooks.Count + 1);
				} else {
					intRoll = random.Next(1, 7);
				}
				
				txbDiceValue.Text = intRoll.ToString();

				for (int i = 1; i <= intRoll; i++) {
					TextBox txt = this.FindName("txt" + i) as TextBox;
					txt.Visibility = Visibility.Visible;
				}

				for (int i = 6; i > intRoll; i--) {
					TextBox txt = this.FindName("txt" + i) as TextBox;
					txt.Visibility = Visibility.Hidden;
				}

				txt1.Focus();

				stopwatchTurn.Start();
				timerTurn.Start();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
