﻿using System;
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

using BibleBooksWPF.Helpers;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for GreekReorder.xaml
	/// </summary>
	public partial class GreekReorder : Page {
		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<string, Point>();

		private static int intNumberCorrect = 0;
		private static int intCurrentPoints = 0;
		private static int intTries = 0;
		List<Point> lpntLabels = new List<Point>();

		static string[] astrGreek = { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
									"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
									"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lbl3John", "lblJude", "lblRevelation" };

		static string[] astrReorderLbls = { "lbl1", "lbl2", "lbl3", "lbl4", "lbl5", "lbl6", "lbl7", "lbl8", "lbl9", "lbl10", "lbl11", "lbl12", "lbl13", "lbl14",
										 "lbl15", "lbl16", "lbl17", "lbl18", "lbl19", "lbl20", "lbl21", "lbl22", "lbl23", "lbl24", "lbl25", "lbl26", "lbl27"};

		static string[] astrChinese = { "马太福音", "马可福音", "路加福音", "约翰福音", "使徒行传", "罗马书", "哥林多前书", "哥林多后书", "加拉太书", "以弗所书", "腓立比书", "歌罗西书", "帖撒罗尼迦前书",
										"帖撒罗尼迦后书", "提摩太前书", "提摩太后书", "提多书", "腓利门书", "希伯来书", "雅各书", "彼得前书", "彼得后书", "约翰一书", "约翰二书", "约翰三书", "犹大书", "启示录"};

		List<string> lstrBooksToComplete = new List<string>(astrGreek);
		DispatcherTimer timer1 = new DispatcherTimer();
		Stopwatch stopwatch = new Stopwatch();

		public GreekReorder() {
			try {
				InitializeComponent();
				LanguageResources.SetDefaultLanguage(this);

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
				int intGridWidth = Layout.TransformToPixels(grdGreekReorder, grdGreekReorder.ActualWidth);
				int intGridHeight = Layout.TransformToPixels(grdGreekReorder, grdGreekReorder.ActualHeight);

				int intLabelWidth = Layout.TransformToPixels(grdGreekReorder, lblActiveElement.ActualWidth);
				int intLabelHeight = Layout.TransformToPixels(grdGreekReorder, lblActiveElement.ActualHeight);

				int intMenuWidth = Layout.TransformToPixels(grdGreekReorder, menTop.ActualWidth);
				int intMenuHeight = Layout.TransformToPixels(grdGreekReorder, menTop.ActualHeight);

				Point pntGrid = grdGreekReorder.PointToScreen(grdGreekReorder.TranslatePoint(new Point(0, 0), this));
				mouseOnElement = new Point(Layout.TransformToPixels(grdGreekReorder, mouseOnElement.X), Layout.TransformToPixels(grdGreekReorder, mouseOnElement.Y));
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
				if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
					// Score labels
					txbCurrentPoints.Text = "本次分数";
					txbPercentageCorrect.Text = "本次正确率";
					txbTimeElapsed.Text = "计时";
					txbTotalPoints.Text = "总分";
				}

				lblTotalPoints.Content = Properties.Settings.Default.lngTotalPoints;

				Random r = new Random();

				int i = 0;
				foreach (String strLbl in astrGreek) {
					Label lbl = this.FindName(strLbl) as Label;

					// Check main language
					if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
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
			try {
				// Play audio
				Label lblBook = sender as Label;
				string strRead = lblBook.Content.ToString();

				SpeechSynthesizer synthesizer = new SpeechSynthesizer();
				synthesizer.SetOutputToDefaultAudioDevice();
				PromptBuilder pBuilder = new PromptBuilder();

				if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
					pBuilder.Culture = CultureInfo.GetCultureInfo("zh-CN");
					pBuilder.AppendText(strRead);
				} else if (Properties.Settings.Default.strLanguage.Equals("en-US")) {
					pBuilder.Culture = CultureInfo.GetCultureInfo("en-US");

					if (strRead.Contains("1")) {
						strRead = strRead.Replace("1", "First");
					} else if (strRead.Contains("2")) {
						strRead = strRead.Replace("2", "Second");
					} else if (strRead.Contains("3")) {
						strRead = strRead.Replace("3", "Third");
					} else if (strRead == "Philemon") {
						pBuilder.AppendTextWithPronunciation("Philemon", "faɪlimən");
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
					int intLabelIndex = Array.IndexOf(astrGreek, lbl.Name);
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
				Properties.Settings.Default.lngTotalPoints -= 1;
				Properties.Settings.Default.Save();
				intTries += 1;
				refreshPoints();

				// Label was moved from original position
				if (dctTransform.ContainsKey(lbl.Name) == false) {
					lbl.RenderTransform = new TranslateTransform();
					dctTransform[lbl.Name] = new Point(0, 0);
				} else {
					// Return to previous position before match
					TranslateTransform transform = lbl.RenderTransform as TranslateTransform;
					transform.X = dctTransform[lbl.Name].X;
					transform.Y = dctTransform[lbl.Name].Y;
				}

				lbl.Background = Brushes.Salmon;
				incorrectFlash(lbl);
			} else if (blnCorrect == false && blnAttemptedMatch == false) {
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
			string strRecord = StatisticMethods.AddStatistic("GreekReorder", intCurrentPoints, time);

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
	}
}