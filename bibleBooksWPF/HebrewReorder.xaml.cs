using System;
using System.Collections.Generic;
using System.Globalization;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Windows.Threading;
using System.Diagnostics;

using ExtensionMethods;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for HebrewReorder.xaml
	/// </summary>
	public partial class HebrewReorder : Page {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<string, Point>();

		private static int intNumberCorrect = 0;
		private static int intCurrentPoints = 0;
		private static int intTries = 0;
		List<Point> lpntLabels = new List<Point>();

		static string[] astrHebrew = { "lblGenesis", "lblExodus", "lblLeviticus", "lblNumbers", "lblDeuteronomy", "lblJoshua", "lblJudges", "lblRuth", "lbl1Samuel",
										"lbl2Samuel", "lbl1Kings", "lbl2Kings", "lbl1Chronicles", "lbl2Chronicles", "lblEzra", "lblNehemiah", "lblEsther", "lblJob",
										"lblPsalms", "lblProverbs", "lblEcclesiastes", "lblSongofSolomon", "lblIsaiah", "lblJeremiah", "lblLamentations", "lblEzekiel",
										"lblDaniel", "lblHosea", "lblJoel", "lblAmos", "lblObadiah", "lblJonah", "lblMicah", "lblNahum", "lblHabakkuk", "lblZephaniah",
										"lblHaggai", "lblZechariah", "lblMalachi"};

		static string[] astrReorderLbls = { "lbl1", "lbl2", "lbl3", "lbl4", "lbl5", "lbl6", "lbl7", "lbl8", "lbl9", "lbl10", "lbl11", "lbl12", "lbl13", "lbl14",
										 "lbl15", "lbl16", "lbl17", "lbl18", "lbl19", "lbl20", "lbl21", "lbl22", "lbl23", "lbl24", "lbl25", "lbl26", "lbl27",
										 "lbl28", "lbl29", "lbl30", "lbl31", "lbl32", "lbl33", "lbl34", "lbl35", "lbl36", "lbl37", "lbl38", "lbl39"};

		static string[] astrChinese = { "创世记", "出埃及记", "利未记", "民数记", "申命记", "约书亚记", "士师记", "路得记", "撒母耳记上", "撒母耳记下", "列王纪上", "列王纪下",
										"历代志上", "历代志下", "以斯拉记", "尼希米记", "以斯帖记", "约伯记", "诗篇", "箴言", "传道书", "雅歌", "以赛亚书", "耶利米书", "耶利米哀歌",
										"以西结书", "但以理书", "何西阿书", "约珥书", "阿摩司书", "俄巴底亚书", "约拿书", "弥迦书", "那鸿书", "哈巴谷书", "西番雅书", "哈该书",
										"撒迦利亚书", "玛拉基书"};

		List<string> lstrBooksToComplete = new List<string>(astrHebrew);
		DispatcherTimer timer1 = new DispatcherTimer();
		Stopwatch stopwatch = new Stopwatch();

		public HebrewReorder() {
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
				Point mouseOnElement = Mouse.GetPosition(lblActiveElement);

				lblActiveElement.CaptureMouse();

				lblActiveElement.BringToFront();
				Cursor = Cursors.Hand;

				// Convert wpf size to pixels to fit all dpi percentages
				int intGridWidth = Layout.TransformToPixels(grdHebrewReorder, grdHebrewReorder.ActualWidth);
				int intGridHeight = Layout.TransformToPixels(grdHebrewReorder, grdHebrewReorder.ActualHeight);

				int intLabelWidth = Layout.TransformToPixels(grdHebrewReorder, lblActiveElement.ActualWidth);
				int intLabelHeight = Layout.TransformToPixels(grdHebrewReorder, lblActiveElement.ActualHeight);

				int intMenuWidth = Layout.TransformToPixels(grdHebrewReorder, menTop.ActualWidth);
				int intMenuHeight = Layout.TransformToPixels(grdHebrewReorder, menTop.ActualHeight);

				Point pntGrid = grdHebrewReorder.PointToScreen(grdHebrewReorder.TranslatePoint(new Point(0, 0), this));
				mouseOnElement = new Point(Layout.TransformToPixels(grdHebrewReorder, mouseOnElement.X), Layout.TransformToPixels(grdHebrewReorder, mouseOnElement.Y));
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
				}

				lblTotalPoints.Content = Properties.Settings.Default.lngTotalPoints;

				Random r = new Random();

				int i = 0;
				foreach (string strLbl in astrHebrew) {
					Label lbl = this.FindName(strLbl) as Label;

					// Check main language
					if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
						lbl.Content = astrChinese[i];
						lbl.FontSize = 19;
					}

					// Add draggable label methods
					lbl.MouseLeftButtonDown += new MouseButtonEventHandler(lblMouseLeftButtonDown);
					lbl.MouseMove += new MouseEventHandler(lblMouseMove);
					lbl.MouseLeftButtonUp += new MouseButtonEventHandler(lblMouseLeftButtonUp);

					lbl.Cursor = Cursors.Hand;

					// Add each label's location in the grid to a list of points
					// (Row, Column)
					Point pntGridPosition = new Point(Grid.GetRow(lbl), Grid.GetColumn(lbl));
					lpntLabels.Add(pntGridPosition);

					i++;
				}

				// Randomly shuffle all label locations
				foreach (String strLbl in astrHebrew) {
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
			try {
				// Play audio
				Label lblBook = sender as Label;
				string strRead = lblBook.Content.ToString();

				SpeechSynthesizer synthesizer = new SpeechSynthesizer();
				synthesizer.SetOutputToDefaultAudioDevice();
				PromptBuilder pBuilder = new PromptBuilder();

				if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					pBuilder.Culture = CultureInfo.GetCultureInfo("zh-CN");
					pBuilder.AppendText(strRead);
				} else if (Properties.Settings.Default.strLanguage.Equals("English")) {
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
				}

				synthesizer.SpeakAsync(pBuilder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
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
				Label lblReorder = (Label)this.FindName(strContainerName);

				// Convert the reorder container label to a rect
				Rect rectReorder = new Rect();
				rectReorder.Location = lblReorder.PointToScreen(new Point(0, 0));
				rectReorder.Height = lblReorder.ActualHeight;
				rectReorder.Width = lblReorder.ActualWidth;

				// Only check the label that is touching a reorder rectangle
				if (rectMovedLbl.IntersectsWith(rectReorder)) {
					int intLabelIndex = Array.IndexOf(astrHebrew, lbl.Name);
					blnAttemptedMatch = true;

					// If the correct English label has been matched
					if (strContainerName.Equals("lbl" + (intLabelIndex + 1).ToString())) {
						// Mark boolean flag true first
						// Override the false in case it is touching 2 English labels at once
						blnCorrect = true;

						// Add points
						intCurrentPoints += 1;
						Properties.Settings.Default.lngTotalPoints += 1;
						Properties.Settings.Default.Save();
						intNumberCorrect += 1;
						intTries += 1;
						refreshPoints();

						// Move label to reordered container label
						lbl.RenderTransform = new TranslateTransform();

						// Remove from main grid
						grdHebrewReorder.Children.Remove(lbl);

						// Add to reorder labels grid
						grdReordered.Children.Add(lbl);
						Grid.SetRow(lbl, Grid.GetRow(lblReorder));
						Grid.SetColumn(lbl, Grid.GetColumn(lblReorder));
						lbl.Background = (Brush)(new BrushConverter().ConvertFromString("#B5DBFF"));
						lbl.IsEnabled = false;

						lstrBooksToComplete.Remove(lbl.Name);

						if (intLabelIndex == 1) {
							// Exodus badge
							switch (App.Current.Properties["exodusBadge"].ToString()) {
								case "match":
									// Second game right
									App.Current.Properties["exodusBadge"] = "both";
									break;
								case "":
									// First time getting book right
									App.Current.Properties["exodusBadge"] = "reorder";
									break;
								default:
									// Badge already awarded
									break;
							}
						} else if (intLabelIndex == 7) {
							// Ruth badge
							switch (App.Current.Properties["ruthBadge"].ToString()) {
								case "match":
									// Second game right
									App.Current.Properties["ruthBadge"] = "both";
									break;
								case "":
									// First time getting book right
									App.Current.Properties["ruthBadge"] = "reorder";
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
				intTries += 1;
				Properties.Settings.Default.lngTotalPoints -= 1;
				Properties.Settings.Default.Save();
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
			lblTotalPoints.Content = Properties.Settings.Default.lngTotalPoints.ToString();
			lblPercentageCorrect.Content = String.Format("{0:P2}", (double)intNumberCorrect / intTries);
		}

		private void completedMatching() {
			stopwatch.Stop();
			CustomMessageBox winMsgBox = new CustomMessageBox();

			// Add the game data to statistics json file
			TimeSpan time = new TimeSpan(stopwatch.Elapsed.Hours, stopwatch.Elapsed.Minutes, stopwatch.Elapsed.Seconds);
			string strRecord = Statistics.AddStatistic("HebrewReorder", intCurrentPoints, time);

			string strResponse = "";
			if (strRecord != "") {
				// Record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intTries) + "\n" +
							"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", strRecord, winMsgBox);
			} else {
				// No record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intTries) + "\n" +
							"Time Elapsed: " + lblTimeElapsed.Content, "Congratulations!", "congrats", winMsgBox);
			}

			switch (strResponse) {
				case "Retry":
					HebrewReorder pHebrewReorder = new HebrewReorder();
					NavigationService.Navigate(pHebrewReorder);
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
