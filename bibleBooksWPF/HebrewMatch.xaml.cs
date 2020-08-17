using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Speech.Synthesis;
using System.Diagnostics;

using ExtensionMethods;
using System.Globalization;

namespace BibleBooksWPF
{
    /// <summary>
    /// Interaction logic for HebrewMatch.xaml
    /// </summary>
    public partial class HebrewMatch : Page {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<String, Point>();

		private static int intNumberAttempted = 0;
		private static int intNumberCorrect = 0;
		private static int intCurrentPoints = 0;
		List<Point> lpntChLabels = new List<Point>();

		static string[] astrChHebrew= { "lblChGenesis", "lblChExodus", "lblChLeviticus", "lblChNumbers", "lblChDeuteronomy", "lblChJoshua", "lblChJudges", "lblChRuth", "lblCh1Samuel",
										"lblCh2Samuel", "lblCh1Kings", "lblCh2Kings", "lblCh1Chronicles", "lblCh2Chronicles", "lblChEzra", "lblChNehemiah", "lblChEsther", "lblChJob",
										"lblChPsalms", "lblChProverbs", "lblChEcclesiastes", "lblChSongofSolomon", "lblChIsaiah", "lblChJeremiah", "lblChLamentations", "lblChEzekiel",
										"lblChDaniel", "lblChHosea", "lblChJoel", "lblChAmos", "lblChObadiah", "lblChJonah", "lblChMicah", "lblChNahum", "lblChHabakkuk", "lblChZephaniah",
										"lblChHaggai", "lblChZechariah", "lblChMalachi"};

		static string[] astrHebrew = { "lblGenesis", "lblExodus", "lblLeviticus", "lblNumbers", "lblDeuteronomy", "lblJoshua", "lblJudges", "lblRuth", "lbl1Samuel",
										"lbl2Samuel", "lbl1Kings", "lbl2Kings", "lbl1Chronicles", "lbl2Chronicles", "lblEzra", "lblNehemiah", "lblEsther", "lblJob",
										"lblPsalms", "lblProverbs", "lblEcclesiastes", "lblSongofSolomon", "lblIsaiah", "lblJeremiah", "lblLamentations", "lblEzekiel",
										"lblDaniel", "lblHosea", "lblJoel", "lblAmos", "lblObadiah", "lblJonah", "lblMicah", "lblNahum", "lblHabakkuk", "lblZephaniah",
										"lblHaggai", "lblZechariah", "lblMalachi"};

		List<String> lstrBooksToComplete = new List<String>(astrChHebrew);
		DispatcherTimer timer1 = new DispatcherTimer();
		Stopwatch stopwatch = new Stopwatch();

		public HebrewMatch() {
			try {
				InitializeComponent();

				// Reset points
				intNumberAttempted = 0;
				intNumberCorrect = 0;
				intCurrentPoints = 0;

				// Reset timer
				timer1.Tick += new EventHandler(timer1_Tick);
				timer1.Interval = new TimeSpan(0, 0, 0, 1);
				stopwatch.Reset();
				lblTimeElapsed.Content = "00:00:00";
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void lblMouseLeftButtonDown(object sender, MouseEventArgs e) {
			try {
				blnDragging = true;
				Label lblActiveElement = sender as Label;
				clickPosition = e.GetPosition(this.Parent as UIElement);
				Point mouseOnElement = Mouse.GetPosition(lblActiveElement);

				lblActiveElement.CaptureMouse();

				lblActiveElement.BringToFront();
				Cursor = Cursors.Hand;

				Point pntGrid = grdHebrewMatch.PointToScreen(grdHebrewMatch.TranslatePoint(new Point(0, 0), this));
				Point pntClip = new Point(pntGrid.X + mouseOnElement.X, pntGrid.Y + mouseOnElement.Y + menTop.ActualHeight);

				// Width: Subtract the label width
				// Height: Subtract height of menu bar and the label height
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle((int)(pntClip.X), (int)(pntClip.Y),
												   (int)(grdHebrewMatch.ActualWidth - lblActiveElement.ActualWidth), (int)(grdHebrewMatch.ActualHeight - menTop.ActualHeight - lblActiveElement.ActualHeight));

				// Check audio setting
				// If on, play audio
				if (Properties.Settings.Default.blnAudio == true) {
					playAudio(sender);
				}
			} catch (Exception ex) {
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				MessageBox.Show(ex.Message);
			}
		}

		private void lblMouseMove(object sender, MouseEventArgs e) {
			try {
				Label lblActiveElement = sender as Label;

				if (blnDragging && lblActiveElement != null) {
					Point currentPosition = e.GetPosition(this.Parent as UIElement);

					TranslateTransform transform = lblActiveElement.RenderTransform as TranslateTransform;
					if (transform == null || dctTransform.ContainsKey(lblActiveElement.Name) == false) {
						transform = new TranslateTransform();
						lblActiveElement.RenderTransform = transform;
					}

					// Transform the distance from the current position to the position it was last in when mouse clicked
					transform.X = currentPosition.X - clickPosition.X;
					transform.Y = currentPosition.Y - clickPosition.Y;

					// Label has been moved before
					if (dctTransform.ContainsKey(lblActiveElement.Name)) {
						if (dctTransform.ContainsKey(lblActiveElement.Name)) {
							// Add on the previous transform
							transform.X += dctTransform[lblActiveElement.Name].X;
							transform.Y += dctTransform[lblActiveElement.Name].Y;
						}
					}
				}
			} catch (Exception ex) {
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				MessageBox.Show(ex.Message);
			}
		}

		private void lblMouseLeftButtonUp(object sender, MouseEventArgs e) {
			try {
				blnDragging = false;
				Label lblActiveElement = sender as Label;
				TranslateTransform transform = lblActiveElement.RenderTransform as TranslateTransform;

				// Check if it has been matched to an English book
				if (checkLabelsTouching(sender) == false) {
					// Not matched, add new point to transform
					if (dctTransform.ContainsKey(lblActiveElement.Name)) {
						// A previous transform is already being stored
						dctTransform[lblActiveElement.Name] = new Point(transform.X, transform.Y);
					}
					else if (transform != null) {
						dctTransform.Add(lblActiveElement.Name, new Point(transform.X, transform.Y));
					};
				}

				lblActiveElement.ReleaseMouseCapture();
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				Cursor = Cursors.Arrow;
			} catch (Exception ex) {
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					// Menu bar
					imenMainMenu.Header = "主菜单";
					imenHebrew.Header = "希伯来语经卷";
					imenMatchHebrew.Header = "中英文配对";
					imenReorderHebrew.Header = "排序";
					imenGreek.Header = "希腊语经卷";
					imenMatchGreek.Header = "中英文配对";
					imenReorderGreek.Header = "排序";
					imenStatistics.Header = "成绩";
					imenSettings.Header = "设置";
					imenExit.Header = "退出";
					menTop.FontSize = 16;

					// Score labels
					txbCurrentPoints.Text = "本次分数";
					txbPercentageCorrect.Text = "本次正确率";
					txbTimeElapsed.Text = "计时";
					txbTotalPoints.Text = "总分";
					txbNumberAttempted.Text = "尝试次数";
				}

				lblTotalPoints.Content = Properties.Settings.Default.lngTotalPoints;

				Random r = new Random();

				foreach (String strChLbl in astrChHebrew) {
					Label lblCh = this.FindName(strChLbl) as Label;

					// Check main language
					if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
						string strTemp = lblCh.Content.ToString();

						Label lblEn = this.FindName(strChLbl.Remove(3, 2)) as Label;
						lblCh.Content = lblEn.Content;
						lblEn.Content = strTemp;
					}

					// Add draggable label methods
					lblCh.MouseLeftButtonDown += new MouseButtonEventHandler(lblMouseLeftButtonDown);
					lblCh.MouseMove += new MouseEventHandler(lblMouseMove);
					lblCh.MouseLeftButtonUp += new MouseButtonEventHandler(lblMouseLeftButtonUp);

					lblCh.Cursor = Cursors.Hand;

					// Add each label's location in the grid to a list of points
					// (Row, Column)
					Point pntGridPosition = new Point(Grid.GetRow(lblCh), Grid.GetColumn(lblCh));
					lpntChLabels.Add(pntGridPosition);
				}

				// Randomly shuffle all Chinese label locations
				foreach (String strChLbl in astrChHebrew) {
					Label lblCh = this.FindName(strChLbl) as Label;

					// Assign the label to a random grid position
					int intRandom = r.Next(0, lpntChLabels.Count);
					Grid.SetRow(lblCh, (int)lpntChLabels[intRandom].X);
					Grid.SetColumn(lblCh, (int)lpntChLabels[intRandom].Y);

					// Remove the point so it is not assigned again
					lpntChLabels.Remove(new Point(Grid.GetRow(lblCh), Grid.GetColumn(lblCh)));
				}

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

		private void playAudio(object sender) {
			try {
				Label lblBook = sender as Label;
				string strRead = lblBook.Content.ToString();

				var synthesizer = new SpeechSynthesizer();
				synthesizer.SetOutputToDefaultAudioDevice();
				PromptBuilder pBuilder = new PromptBuilder();

				if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("EN"));

					if (strRead.Contains("1")) {
						strRead = strRead.Replace("1", "fɜrst");
						pBuilder.AppendText(strRead);
					} else if (strRead.Contains("2")) {
						strRead = strRead.Replace("2", "Second");
						pBuilder.AppendText(strRead);
					} else if (strRead.Contains("3")) {
						strRead = strRead.Replace("3", "Third");
						pBuilder.AppendText(strRead);
					} else if (strRead == "Job") {
						pBuilder.AppendTextWithPronunciation("Job", "ʤoʊb");
					} else if (strRead == "Haggai") {
						pBuilder.AppendTextWithPronunciation("Haggai", "hægaɪ");
					} else if (strRead == "Habakkuk") {
						pBuilder.AppendTextWithPronunciation("Habakkuk", "hʌbækʌk");
					} else {
						pBuilder.AppendText(strRead);
					}
				} else if (Properties.Settings.Default.strLanguage.Equals("English")) {
					synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("zh-CN"));
					pBuilder.AppendText(strRead);
				}

				synthesizer.SpeakAsync(pBuilder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private bool checkLabelsTouching(object sender) {
			Label lblCh = sender as Label;
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Turn Chinese label int a rectangle
			Rect rctChLbl = new Rect();
			rctChLbl.Location = lblCh.PointToScreen(new Point(0, 0));
			rctChLbl.Height = lblCh.ActualHeight;
			rctChLbl.Width = lblCh.ActualWidth;

			// Check each English book to see if touching
			foreach (String strLbl in astrHebrew) {
				// Get the label from the string name
				Label lbl = this.FindName(strLbl) as Label;

				// Turn English label into a rectangle
				Rect rctLbl = new Rect();
				rctLbl.Location = lbl.PointToScreen(new Point(0, 0));
				rctLbl.Height = lbl.ActualHeight;
				rctLbl.Width = lbl.ActualWidth;

				// Only check English labels that are touching the Chinese label
				if (rctChLbl.IntersectsWith(rctLbl)) {
					int intChLabelIndex = Array.IndexOf(astrChHebrew, lblCh.Name);
					blnAttemptedMatch = true;

					// If the correct English label has been matched
					if (strLbl == astrHebrew[intChLabelIndex]) {
						// Mark boolean flag true first
						// Override the false in case it is touching 2 English labels at once
						blnCorrect = true;

						// Add points
						intCurrentPoints += 1;
						Properties.Settings.Default.lngTotalPoints += 1;
						Properties.Settings.Default.Save();
						intNumberCorrect += 1;
						intNumberAttempted += 1;
						refreshPoints();

						// Move correct label on top of English
						lblCh.RenderTransform = new TranslateTransform();
						Grid.SetRow(lblCh, Grid.GetRow(lbl));
						Grid.SetColumn(lblCh, Grid.GetColumn(lbl));
						lblCh.IsEnabled = false;

						lstrBooksToComplete.Remove(lblCh.Name);

						if (intChLabelIndex == 1) {
							// Exodus badge
							switch (App.Current.Properties["exodusBadge"].ToString()) {
								case "reorder":
									// Second game right
									App.Current.Properties["exodusBadge"] = "both";
									break;
								case "":
									// First time getting book right
									App.Current.Properties["exodusBadge"] = "match";
									break;
								default:
									// Badge already awarded
									break;
							}
						} else if (intChLabelIndex == 7) {
							// Ruth badge
							switch (App.Current.Properties["ruthBadge"].ToString()) {
								case "reorder":
									// Second game right
									App.Current.Properties["ruthBadge"] = "both";
									break;
								case "":
									// First time getting book right
									App.Current.Properties["ruthBadge"] = "match";
									break;
								default:
									// Badge already awarded
									break;
							}
						}

						// Check if all books have been matched
						if (lstrBooksToComplete.Count == 0) {
							// Show congratulations message
							completedMatching();
						}
					}
				}
			}

			if (blnCorrect == false && blnAttemptedMatch == true) {
				// Point penalty
				intCurrentPoints -= 1;
				Properties.Settings.Default.lngTotalPoints -= 1;
				Properties.Settings.Default.Save();
				intNumberAttempted += 1;
				refreshPoints();

				// Label was moved from original position
				if (dctTransform.ContainsKey(lblCh.Name) == false) {
					lblCh.RenderTransform = new TranslateTransform();
					dctTransform[lblCh.Name] = new Point(0, 0);
				}
				else {
					// Return to previous position before match
					TranslateTransform transform = lblCh.RenderTransform as TranslateTransform;
					transform.X = dctTransform[lblCh.Name].X;
					transform.Y = dctTransform[lblCh.Name].Y;
				}

				lblCh.Background = Brushes.Salmon;
				incorrectFlash(lblCh);
			}
			else if (blnCorrect == false && blnAttemptedMatch == false) {
				// No match attempted
				return false;
			}

			// Match was attempted, whether correct or not
			return true;
		}

		private async void incorrectFlash(Label lblIncorrectBook) {
			await Task.Delay(900);
			lblIncorrectBook.Background = (Brush)(new BrushConverter().ConvertFromString("#E6EBF3"));
		}

		private void refreshPoints() {
			lblCurrentPoints.Content = intCurrentPoints.ToString();
			lblTotalPoints.Content = Properties.Settings.Default.lngTotalPoints.ToString();
			lblPercentageCorrect.Content = String.Format("{0:P2}", (double)intNumberCorrect / intNumberAttempted);
			lblNumberAttempted.Content = intNumberAttempted.ToString();
		}

		private void completedMatching() {
			stopwatch.Stop();
			CustomMessageBox winMsgBox = new CustomMessageBox();

			// Add the game data to statistics json file
			TimeSpan time = new TimeSpan(stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
			string strRecord = Statistics.AddStatistic("HebrewMatch", intCurrentPoints, time);

			string strResponse = "";
			if (strRecord != "") {
				// Record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intNumberAttempted) + "\n" +
							"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", strRecord, winMsgBox);
			} else {
				// No record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intNumberAttempted) + "\n" +
							"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", winMsgBox);
			}

			switch (strResponse) {
				case "Retry":
					HebrewMatch pHebrewMatch = new HebrewMatch();
					NavigationService.Navigate(pHebrewMatch);
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

		private void btnPause_Click(object sender, RoutedEventArgs e) {
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
		}

		private void ImenMainMenu_Click(object sender, RoutedEventArgs e) {
			try {
				MainMenu pMainMenu = new MainMenu();
				NavigationService.Navigate(pMainMenu);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenReorderHebrew_Click(object sender, RoutedEventArgs e) {
			try {
				HebrewReorder pHebrewReorder = new HebrewReorder();
				NavigationService.Navigate(pHebrewReorder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenMatchGreek_Click(object sender, RoutedEventArgs e) {
			try {
				GreekMatch pGreekMatch = new GreekMatch();
				NavigationService.Navigate(pGreekMatch);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenReorderGreek_Click(object sender, RoutedEventArgs e) {
			try {
				GreekReorder pGreekReorder = new GreekReorder();
				NavigationService.Navigate(pGreekReorder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenSettings_Click(object sender, RoutedEventArgs e) {
			try {
				Settings pSettings = new Settings();
				NavigationService.Navigate(pSettings);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenStatistics_Click(object sender, RoutedEventArgs e) {
			try {
				StatisticsPage pStatistics = new StatisticsPage();
				NavigationService.Navigate(pStatistics);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}
	}
}
