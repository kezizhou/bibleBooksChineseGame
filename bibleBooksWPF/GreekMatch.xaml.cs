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

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for GreekMatch.xaml
	/// </summary>
	public partial class GreekMatch : Page {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		private Point labelClickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<String, Point>();

		private Point gridBeforeMatch;
		private static int intGreekAnswered = 0;
		private static int intNumberCorrect = 0;
		private static int intGreekPoints = 0;
		private static TimeSpan tsSecondsElapsed = TimeSpan.FromSeconds(0);
		List<Point> lpntChLabels = new List<Point>();

		static string[] astrChGreek = { "lblChMatthew", "lblChMark", "lblChLuke", "lblChJohn", "lblChActs", "lblChRomans", "lblCh1Corinthians", "lblCh2Corinthians", "lblChGalatians",
									"lblChEphesians", "lblChPhilippians", "lblChColossians", "lblCh1Thessalonians", "lblCh2Thessalonians", "lblCh1Timothy", "lblCh2Timothy", "lblChTitus",
									"lblChPhilemon", "lblChHebrews", "lblChJames", "lblCh1Peter", "lblCh2Peter", "lblCh1John", "lblCh2John", "lblCh3John", "lblChJude", "lblChRevelation" };

		static string[] astrGreek = { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
									"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
									"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lbl3John", "lblJude", "lblRevelation" };

		List<String> lstrBooksToComplete = new List<String>(astrChGreek);
		DispatcherTimer timer1 = new DispatcherTimer();

		public GreekMatch() {
			InitializeComponent();
		}

		private void lblMouseDown(object sender, MouseEventArgs e) {
			blnDragging = true;
			Label lblActiveElement = sender as Label;
			clickPosition = e.GetPosition(this.Parent as UIElement);
			labelClickPosition = lblActiveElement.TransformToAncestor(grdGreekMatch).Transform(new Point(0, 0));
			gridBeforeMatch = new Point(Grid.GetRow(lblActiveElement), Grid.GetColumn(lblActiveElement));

			lblActiveElement.CaptureMouse();

			lblActiveElement.BringToFront();
			Cursor = Cursors.Hand;

			// Check audio setting
			// If on, play audio
			if ((bool)App.Current.Properties["blnAudio"] == true) {
				playChineseAudio(sender);
			}
		}

		private void lblMouseMove(object sender, MouseEventArgs e) {
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
		}

		private void lblMouseUp(object sender, MouseEventArgs e) {
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
				else {
					dctTransform.Add(lblActiveElement.Name, new Point(transform.X, transform.Y));
				};
			}

			lblActiveElement.ReleaseMouseCapture();
			Cursor = Cursors.Arrow;

		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			Random r = new Random();

			foreach (String strChLbl in astrChGreek) {
				// Add draggable label methods
				Label lblCh = this.FindName(strChLbl) as Label;
				lblCh.MouseDown += new MouseButtonEventHandler(lblMouseDown);
				lblCh.MouseMove += new MouseEventHandler(lblMouseMove);
				lblCh.MouseUp += new MouseButtonEventHandler(lblMouseUp);

				// Add each label's location in the grid to a list of points
				// (Row, Column)
				Point pntGridPosition = new Point(Grid.GetRow(lblCh), Grid.GetColumn(lblCh));
				lpntChLabels.Add(pntGridPosition);
			}

			// Randomly shuffle all Chinese label locations
			foreach (String strChLbl in astrChGreek) {
				Label lblCh = this.FindName(strChLbl) as Label;

				// Assign the label to a random grid position
				int intRandom = r.Next(0, lpntChLabels.Count);
				Grid.SetRow(lblCh, (int) lpntChLabels[intRandom].X);
				Grid.SetColumn(lblCh, (int) lpntChLabels[intRandom].Y);

				// Remove the point so it is not assigned again
				lpntChLabels.Remove( new Point(Grid.GetRow(lblCh), Grid.GetColumn(lblCh)) );
			}

			// Start timer
			timer1.Interval = TimeSpan.FromSeconds(1);
			timer1.Tick += timer1_Tick;
			timer1.Start();
		}

		private void timer1_Tick(object sender, EventArgs e) {
			tsSecondsElapsed += TimeSpan.FromSeconds(1);
			lblTimeElapsed.Content= tsSecondsElapsed.ToString();
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
			foreach (String strLbl in astrGreek) {
				// Get the label from the string name
				Label lbl = this.FindName(strLbl) as Label;

				// Turn English label into a rectangle
				Rect rctLbl = new Rect();
				rctLbl.Location = lbl.PointToScreen(new Point(0, 0));
				rctLbl.Height = lbl.ActualHeight;
				rctLbl.Width = lbl.ActualWidth;

				// Only check English labels that are touching the Chinese label
				if (rctChLbl.IntersectsWith(rctLbl)) {
					int intChLabelIndex = Array.IndexOf(astrChGreek, lblCh.Name);
					blnAttemptedMatch = true;
					
					// If the correct English label has been matched
					if (strLbl == astrGreek[intChLabelIndex]) {
						// Mark boolean flag true first
						// Override the false in case it is touching 2 English labels at once
						blnCorrect = true;

						// Add points
						intGreekPoints += 1;
						MainMenu.intTotalPoints += 1;
						intNumberCorrect += 1;
						intGreekAnswered += 1;
						refreshPoints();

						// Move correct label on top of English
						lblCh.RenderTransform = new TranslateTransform();
						Grid.SetRow(lblCh, Grid.GetRow(lbl));
						Grid.SetColumn(lblCh, Grid.GetColumn(lbl));
						lblCh.IsEnabled = false;

						lstrBooksToComplete.Remove(lblCh.Name);

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
				intGreekPoints -= 1;
				MainMenu.intTotalPoints -= 1;
				intGreekAnswered += 1;
				refreshPoints();

				// Label was moved from original position
				if (dctTransform.ContainsKey(lblCh.Name) == false) {
					lblCh.RenderTransform = new TranslateTransform();
					dctTransform[lblCh.Name] = new Point(0, 0);
				} else {
					// Return to previous position before match
					TranslateTransform transform = lblCh.RenderTransform as TranslateTransform;
					transform.X = dctTransform[lblCh.Name].X;
					transform.Y = dctTransform[lblCh.Name].Y;
				}

				lblCh.Background = Brushes.Salmon;
				incorrectFlash(lblCh);
			} else if(blnCorrect == false && blnAttemptedMatch == false) {
				// No match attempted
				return false;
			}

			// Match was attempted, whether correct or not
			return true;
		}

		private async void incorrectFlash(Label lblIncorrectBook) {
			await Task.Delay(900);
			lblIncorrectBook.Background = (Brush)(new BrushConverter().ConvertFromString("#FFE6EBF3"));
		}

		private void refreshPoints() {
			lblGreekPoints.Content = intGreekPoints.ToString();
			lblTotalPoints.Content = MainMenu.intTotalPoints.ToString();
			lblPercentageCorrect.Content = String.Format("{0:P2}", (double)intNumberCorrect / intGreekAnswered);
			lblGreekAnswered.Content = intGreekAnswered.ToString();
		}

		private void completedMatching() {
			timer1.IsEnabled = false;
			CustomMessageBox winMsgBox = new CustomMessageBox();

			string strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
										"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intGreekAnswered) + "\n" +
										"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", winMsgBox);

			switch (strResponse) {
				case "Retry":
					GreekMatch pGreekMatch = new GreekMatch();
					NavigationService.Navigate(pGreekMatch);
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
	}
}
