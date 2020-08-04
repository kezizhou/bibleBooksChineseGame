using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Speech.Synthesis;

using ExtensionMethods;
using System.Globalization;
using System.Windows.Threading;
using System.Diagnostics;
using System.Threading;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for GreekReorder.xaml
	/// </summary>
	public partial class GreekReorder : Page {
		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		private Point labelClickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<string, Point>();

		private Point gridBeforeMatch;
		private static int intNumberCorrect = 0;
		private static int intCurrentPoints = 0;
		private static int intTries = 0;
		private static TimeSpan tsSecondsElapsed = TimeSpan.FromSeconds(0);
		List<Point> lpntLabels = new List<Point>();

		static string[] astrGreek = { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
									"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
									"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lbl3John", "lblJude", "lblRevelation" };

		static string[] astrReorderLbls = { "lbl1", "lbl2", "lbl3", "lbl4", "lbl5", "lbl6", "lbl7", "lbl8", "lbl9", "lbl10", "lbl11", "lbl12", "lbl13", "lbl14",
										 "lbl15", "lbl16", "lbl17", "lbl18", "lbl19", "lbl20", "lbl21", "lbl22", "lbl23", "lbl24", "lbl25", "lbl26", "lbl27"};

		List<string> lstrBooksToComplete = new List<string>(astrGreek);
		DispatcherTimer timer1 = new DispatcherTimer();
		Stopwatch stopwatch = new Stopwatch();

		public GreekReorder() {
			try {
				InitializeComponent();

				// Reset points
				intNumberCorrect = 0;
				intCurrentPoints = 0;
				intTries = 0;

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
				labelClickPosition = lblActiveElement.TransformToAncestor(grdGreekReorder).Transform(new Point(0, 0));
				gridBeforeMatch = new Point(Grid.GetRow(lblActiveElement), Grid.GetColumn(lblActiveElement));

				lblActiveElement.CaptureMouse();

				lblActiveElement.BringToFront();
				Cursor = Cursors.Hand;

				// Check audio setting
				// If on, play audio
				if (Properties.Settings.Default.blnAudio == true) {
					playAudio(sender);
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

				foreach (String strLbl in astrGreek) {
					// Add draggable label methods
					Label lbl = this.FindName(strLbl) as Label;
					lbl.MouseLeftButtonDown += new MouseButtonEventHandler(lblMouseLeftButtonDown);
					lbl.MouseMove += new MouseEventHandler(lblMouseMove);
					lbl.MouseLeftButtonUp += new MouseButtonEventHandler(lblMouseLeftButtonUp);

					// Add each label's location in the grid to a list of points
					// (Row, Column)
					Point pntGridPosition = new Point(Grid.GetRow(lbl), Grid.GetColumn(lbl));
					lpntLabels.Add(pntGridPosition);
				}

				// Randomly shuffle all label locations
				foreach (String strLbl in astrGreek) {
					Label lbl = this.FindName(strLbl) as Label;

					// Assign the label to a random grid position
					int intRandom = r.Next(0, lpntLabels.Count);
					Grid.SetRow(lbl, (int)lpntLabels[intRandom].X);
					Grid.SetColumn(lbl, (int)lpntLabels[intRandom].Y);

					// Remove the point so it is not assigned again
					lpntLabels.Remove(new Point(Grid.GetRow(lbl), Grid.GetColumn(lbl)));
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
			// Play audio
			Label lblBook = sender as Label;
			var synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("en"));
			synthesizer.SpeakAsync(lblBook.Content.ToString());
		}

		private bool checkLabelsTouching(object sender) {
			Label lbl = sender as Label;
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Convert the moved label into a rect
			Rect rectMovedLbl = new Rect();
			rectMovedLbl.Location = lbl.PointToScreen(new Point(0, 0));
			rectMovedLbl.Height = lbl.ActualHeight;
			rectMovedLbl.Width = lbl.ActualWidth;

			// Loop through the container labels to see if the moved label is touching one of them
			foreach (String strContainerName in astrReorderLbls) {
				// Get the label from the string name
				Label lblReorder = (Label) this.FindName(strContainerName);

				// Convert the reorder container label to a rect
				Rect rectReorder = new Rect();
				rectReorder.Location = lblReorder.PointToScreen(new Point(0, 0));
				rectReorder.Height = lblReorder.ActualHeight;
				rectReorder.Width = lblReorder.ActualWidth;

				// Only check the label that is touching a reorder rectangle
				if (rectMovedLbl.IntersectsWith(rectReorder)) {
					int intLabelIndex = Array.IndexOf(astrGreek, lbl.Name);
					blnAttemptedMatch = true;

					// If the correct English label has been matched
					if (strContainerName.Equals( "lbl" + (intLabelIndex + 1).ToString() )) {
						// Mark boolean flag true first
						// Override the false in case it is touching 2 English labels at once
						blnCorrect = true;

						// Add points
						intCurrentPoints += 1;
						MainMenu.intTotalPoints += 1;
						intNumberCorrect += 1;
						intTries += 1;
						refreshPoints();

						// Move label to reordered container label
						lbl.RenderTransform = new TranslateTransform();

						// Remove from main grid
						grdGreekReorder.Children.Remove(lbl);

						// Add to reorder labels grid
						grdReordered.Children.Add(lbl);
						Grid.SetRow(lbl, Grid.GetRow(lblReorder));
						Grid.SetColumn(lbl, Grid.GetColumn(lblReorder));
						lbl.Background = (Brush)(new BrushConverter().ConvertFromString("#B5DBFF"));
						lbl.IsEnabled = false;

						lstrBooksToComplete.Remove(lbl.Name);

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
				intTries += 1;
				refreshPoints();

				// Label was moved from original position
				if (dctTransform.ContainsKey(lbl.Name) == false) {
					lbl.RenderTransform = new TranslateTransform();
					dctTransform[lbl.Name] = new Point(0, 0);
				}
				else {
					// Return to previous position before match
					TranslateTransform transform = lbl.RenderTransform as TranslateTransform;
					transform.X = dctTransform[lbl.Name].X;
					transform.Y = dctTransform[lbl.Name].Y;
				}

				lbl.Background = Brushes.Salmon;
				incorrectFlash(lbl);
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
			lblIncorrectBook.Background = (Brush)(new BrushConverter().ConvertFromString("#9BC1FF"));
		}

		private void refreshPoints() {
			lblCurrentPoints.Content = intCurrentPoints.ToString();
			lblTotalPoints.Content = MainMenu.intTotalPoints.ToString();
			lblPercentageCorrect.Content = String.Format("{0:P2}", (double)intNumberCorrect / intTries);
		}

		private void completedMatching() {
			stopwatch.Stop();
			CustomMessageBox winMsgBox = new CustomMessageBox();

			// Add the game data to statistics json file
			Statistics.AddStatistic("GreekReorder", intCurrentPoints, tsSecondsElapsed);

			string strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
										"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intTries) + "\n" +
										"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", winMsgBox);

			switch (strResponse) {
				case "Retry":
					GreekReorder pGreekReorder = new GreekReorder();
					NavigationService.Navigate(pGreekReorder);
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

		private void ImenMatchHebrew_Click(object sender, RoutedEventArgs e) {
			try {
				HebrewMatch pHebrewMatch = new HebrewMatch();
				NavigationService.Navigate(pHebrewMatch);
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

		private void ImenStatistics_Click(object sender, RoutedEventArgs e) {
			try {
				StatisticsPage pStatistics = new StatisticsPage();
				NavigationService.Navigate(pStatistics);
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

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}
	}
}
