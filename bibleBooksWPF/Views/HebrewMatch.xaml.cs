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
using System.Globalization;

using BibleBooksWPF.ViewModels;
using BibleBooksWPF.Helpers;

namespace BibleBooksWPF.Views {
    /// <summary>
    /// Interaction logic for HebrewMatch.xaml
    /// </summary>
    public partial class HebrewMatch : ContentControl {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		MatchingGameViewModel viewModel;
		Dictionary<string, Point> dctTransform = new Dictionary<String, Point>();

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
				LanguageResources.SetDefaultLanguage(this);

				// Reset points
				viewModel = new MatchingGameViewModel();
				this.DataContext = viewModel;

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

				// Convert wpf size to pixels to fit all dpi percentages
				int intGridWidth = Layout.TransformToPixels(grdHebrewMatch, grdHebrewMatch.ActualWidth);
				int intGridHeight = Layout.TransformToPixels(grdHebrewMatch, grdHebrewMatch.ActualHeight);

				int intLabelWidth = Layout.TransformToPixels(grdHebrewMatch, lblActiveElement.ActualWidth);
				int intLabelHeight = Layout.TransformToPixels(grdHebrewMatch, lblActiveElement.ActualHeight);

				int intMenuWidth = Layout.TransformToPixels(grdHebrewMatch, ((MainWindow)App.Current.MainWindow).menTop.ActualWidth);
				int intMenuHeight = Layout.TransformToPixels(grdHebrewMatch, ((MainWindow)App.Current.MainWindow).menTop.ActualHeight);

				Point pntGrid = grdHebrewMatch.PointToScreen(grdHebrewMatch.TranslatePoint(new Point(0, 0), this));
				mouseOnElement = new Point(Layout.TransformToPixels(grdHebrewMatch, mouseOnElement.X), Layout.TransformToPixels(grdHebrewMatch, mouseOnElement.Y));
				Point pntClip = new Point(pntGrid.X + mouseOnElement.X, pntGrid.Y + mouseOnElement.Y + intMenuHeight);

				// Width: Subtract the label width
				// Height: Subtract height of menu bar and the label height
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle((int)(pntClip.X), (int)(pntClip.Y), intGridWidth - intLabelWidth, intGridHeight - intMenuHeight - intLabelHeight);

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
				if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
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
					if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
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
					viewModel.propTimeElapsed = stopwatch.Elapsed;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void playAudio(object sender) {
			try {
				Label lblBook = sender as Label;
				string strRead = lblBook.Content.ToString();

				SpeechSynthesizer synthesizer = new SpeechSynthesizer();
				synthesizer.SetOutputToDefaultAudioDevice();
				PromptBuilder pBuilder = new PromptBuilder();

				if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
					pBuilder.Culture = CultureInfo.GetCultureInfo("en-US");

					if (strRead.Contains("1")) {
						strRead = strRead.Replace("1", "First");
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
				} else if (Properties.Settings.Default.strLanguage.Equals("en-US")) {
					pBuilder.Culture = CultureInfo.GetCultureInfo("zh-CN");
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

				var tpMatchReturn = MatchingGames.checkTouchingLabelsCorrect(lbl, lblCh, rctChLbl, astrHebrew, astrChHebrew);
				if (tpMatchReturn.Item1) blnCorrect = true;
				if (tpMatchReturn.Item2) blnAttemptedMatch = true;

				lstrBooksToComplete.Remove(lblCh.Name);
				if (blnCorrect) {
					// Add points
					viewModel.AddCorrectAttempt();

					lstrBooksToComplete.Remove(lblCh.Name);

					int intChLabelIndex = Array.IndexOf(astrChHebrew, lblCh.Name);
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

					// Finished matching
					if (lstrBooksToComplete.Count == 0) {
						stopwatch.Stop();
						string strResponse = MatchingGames.completedMatching(stopwatch.Elapsed, viewModel.propCurrentPoints, viewModel.propNumberCorrect, viewModel.propNumberAttempted);

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

					break;
				}
			}

			if (blnCorrect == false && blnAttemptedMatch == true) {
				// Point penalty
				viewModel.AddIncorrectAttempt(); ;

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

		private void btnPause_Click(object sender, RoutedEventArgs e) {
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
	}
}
