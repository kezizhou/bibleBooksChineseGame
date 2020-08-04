﻿using System;
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
		private Point labelClickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<String, Point>();

		private Point gridBeforeMatch;
		private static int intHebrewAnswered = 0;
		private static int intNumberCorrect = 0;
		private static int intCurrentPoints = 0;
		private static TimeSpan tsSecondsElapsed = TimeSpan.FromSeconds(0);
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
				intHebrewAnswered = 0;
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
				labelClickPosition = lblActiveElement.TransformToAncestor(grdHebrewMatch).Transform(new Point(0, 0));
				gridBeforeMatch = new Point(Grid.GetRow(lblActiveElement), Grid.GetColumn(lblActiveElement));

				lblActiveElement.CaptureMouse();

				lblActiveElement.BringToFront();
				Cursor = Cursors.Hand;

				// Check audio setting
				// If on, play audio
				if (Properties.Settings.Default.blnAudio == true) {
					playChineseAudio(sender);
				}
			} catch (Exception ex) {
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
				Cursor = Cursors.Arrow;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				Random r = new Random();

				foreach (String strChLbl in astrChHebrew) {
					// Add draggable label methods
					Label lblCh = this.FindName(strChLbl) as Label;
					lblCh.MouseLeftButtonDown += new MouseButtonEventHandler(lblMouseLeftButtonDown);
					lblCh.MouseMove += new MouseEventHandler(lblMouseMove);
					lblCh.MouseLeftButtonUp += new MouseButtonEventHandler(lblMouseLeftButtonUp);

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

		private void playChineseAudio(object sender) {
			// Play audio
			Label lblChineseBook = sender as Label;
			var synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("zh-CN"));
			synthesizer.SpeakAsync(lblChineseBook.Content.ToString());
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
						MainMenu.intTotalPoints += 1;
						intNumberCorrect += 1;
						intHebrewAnswered += 1;
						refreshPoints();

						// Move correct label on top of English
						lblCh.RenderTransform = new TranslateTransform();
						Grid.SetRow(lblCh, Grid.GetRow(lbl));
						Grid.SetColumn(lblCh, Grid.GetColumn(lbl));
						lblCh.IsEnabled = false;

						lstrBooksToComplete.Remove(lblCh.Name);

						if (intChLabelIndex == 1) {
							// Exodus badge
							if (App.Current.Properties["exodusBadge"].Equals("1")) {
								App.Current.Properties["exodusBadge"] = "2";
							} else {
								App.Current.Properties["exodusBadge"] = "1";
							}
						} else if (intChLabelIndex == 7) {
							// Ruth badge
							if (App.Current.Properties["ruthBadge"].Equals("1")) {
								App.Current.Properties["ruthBadge"] = "2";
							}
							else {
								App.Current.Properties["ruthBadge"] = "1";
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
				MainMenu.intTotalPoints -= 1;
				intHebrewAnswered += 1;
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
			lblTotalPoints.Content = MainMenu.intTotalPoints.ToString();
			lblPercentageCorrect.Content = String.Format("{0:P2}", (double)intNumberCorrect / intHebrewAnswered);
			lblHebrewAnswered.Content = intHebrewAnswered.ToString();
		}

		private void completedMatching() {
			stopwatch.Stop();
			CustomMessageBox winMsgBox = new CustomMessageBox();

			// Add the game data to statistics json file
			Statistics.AddStatistic("HebrewMatch", intCurrentPoints, tsSecondsElapsed);

			string strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
										"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intHebrewAnswered) + "\n" +
										"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", winMsgBox);

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
