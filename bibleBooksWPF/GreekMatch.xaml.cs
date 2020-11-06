using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Speech.Synthesis;
using System.Globalization;
using System.Windows.Threading;
using System.Diagnostics;

using ExtensionMethods;
using BibleBooksWPF.ViewModels;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for GreekMatch.xaml
	/// </summary>
	public partial class GreekMatch : Page {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		MatchingGameViewModel viewModel;
		Dictionary<string, Point> dctTransform = new Dictionary<String, Point>();

		List<Point> lpntChLabels = new List<Point>();

		static string[] astrChGreek = { "lblChMatthew", "lblChMark", "lblChLuke", "lblChJohn", "lblChActs", "lblChRomans", "lblCh1Corinthians", "lblCh2Corinthians", "lblChGalatians",
									"lblChEphesians", "lblChPhilippians", "lblChColossians", "lblCh1Thessalonians", "lblCh2Thessalonians", "lblCh1Timothy", "lblCh2Timothy", "lblChTitus",
									"lblChPhilemon", "lblChHebrews", "lblChJames", "lblCh1Peter", "lblCh2Peter", "lblCh1John", "lblCh2John", "lblCh3John", "lblChJude", "lblChRevelation" };

		static string[] astrGreek = { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
									"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
									"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lbl3John", "lblJude", "lblRevelation" };

		List<String> lstrBooksToComplete = new List<String>(astrChGreek);
		DispatcherTimer timer1 = new DispatcherTimer();
		Stopwatch stopwatch = new Stopwatch();

		public GreekMatch() {
			try {
				InitializeComponent();

				// Reset points
				viewModel = new MatchingGameViewModel();
				this.DataContext = viewModel;

				// Reset timer
				timer1.Tick += new EventHandler(timer1_Tick);
				timer1.Interval = new TimeSpan(0, 0, 0, 1);
				stopwatch.Reset();
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
				int intGridWidth = Layout.TransformToPixels(grdGreekMatch, grdGreekMatch.ActualWidth);
				int intGridHeight = Layout.TransformToPixels(grdGreekMatch, grdGreekMatch.ActualHeight);

				int intLabelWidth = Layout.TransformToPixels(grdGreekMatch, lblActiveElement.ActualWidth);
				int intLabelHeight = Layout.TransformToPixels(grdGreekMatch, lblActiveElement.ActualHeight);

				int intMenuWidth = Layout.TransformToPixels(grdGreekMatch, menTop.ActualWidth);
				int intMenuHeight = Layout.TransformToPixels(grdGreekMatch, menTop.ActualHeight);

				Point pntGrid = grdGreekMatch.PointToScreen(grdGreekMatch.TranslatePoint(new Point(0, 0), this));
				mouseOnElement = new Point(Layout.TransformToPixels(grdGreekMatch, mouseOnElement.X), Layout.TransformToPixels(grdGreekMatch, mouseOnElement.Y));
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
					Point mouseOnElement = Mouse.GetPosition(lblActiveElement);

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
					} else if (transform != null) {
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

				Random r = new Random();

				foreach (String strChLbl in astrChGreek) {
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

				if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
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
					} else if (strRead == "Philemon") {
						pBuilder.AppendTextWithPronunciation("Philemon", "faɪlimən");
					} else {
						pBuilder.AppendText(strRead);
					}
				} else if (Properties.Settings.Default.strLanguage.Equals("English")) {
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

			// Turn Chinese label into a rectangle
			Rect rctChLbl = new Rect();
			rctChLbl.Location = lblCh.PointToScreen(new Point(0, 0));
			rctChLbl.Height = lblCh.ActualHeight;
			rctChLbl.Width = lblCh.ActualWidth;

			// Check each English book to see if touching
			foreach (String strLbl in astrGreek) {
				// Get the label from the string name
				Label lbl = this.FindName(strLbl) as Label;

				var tpMatchReturn = MatchingGames.checkTouchingLabelsCorrect(lbl, lblCh, rctChLbl, astrGreek, astrChGreek);
				if (tpMatchReturn.Item1) blnCorrect = true;
				if (tpMatchReturn.Item2) blnAttemptedMatch = true;

				if (blnCorrect) {
					// Add points
					viewModel.AddCorrectAttempt();

					lstrBooksToComplete.Remove(lblCh.Name);

					// Finished matching
					if (lstrBooksToComplete.Count == 0) {
						stopwatch.Stop();
						string strResponse = MatchingGames.completedMatching(stopwatch.Elapsed, viewModel.propCurrentPoints, viewModel.propNumberCorrect, viewModel.propNumberAttempted);

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

					break;
				}
			}

			if (blnCorrect == false && blnAttemptedMatch == true) {
				// Point penalty
				viewModel.AddIncorrectAttempt();

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

		private void ImenReorderGreek_Click(object sender, RoutedEventArgs e) {
			try {
				GreekReorder pGreekReorder = new GreekReorder();
				NavigationService.Navigate(pGreekReorder);
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
