using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using System.Diagnostics;

using BibleBooksWPF.ViewModels;
using BibleBooksWPF.Helpers;
using BibleBooksWPF.UserControls;

namespace BibleBooksWPF.Views {
    /// <summary>
    /// Interaction logic for HebrewMatch.xaml
    /// </summary>
    public partial class HebrewMatch : ContentControl {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		HebrewMatchViewModel viewModel;
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
				viewModel = new HebrewMatchViewModel();
				this.DataContext = viewModel;
			} catch (Exception ex) {
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
					BibleBook lblCh = this.FindName(strChLbl) as BibleBook;

					// Check main language
					if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
						string strTemp = lblCh.SetText.ToString();

						BibleBook lblEn = this.FindName(strChLbl.Remove(3, 2)) as BibleBook;
						lblCh.SetText = lblEn.SetText;
						lblEn.SetText = strTemp;
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
					BibleBook lblCh = this.FindName(strChLbl) as BibleBook;

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

		private void lblMouseLeftButtonDown(object sender, MouseEventArgs e) {
			try {
				blnDragging = true;
				BibleBook lblActiveElement = sender as BibleBook;
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

				Point pntGrid = grdHebrewMatch.PointToScreen(grdHebrewMatch.TranslatePoint(new Point(0, 0), this));
				mouseOnElement = new Point(Layout.TransformToPixels(grdHebrewMatch, mouseOnElement.X), Layout.TransformToPixels(grdHebrewMatch, mouseOnElement.Y));
				Point pntClip = new Point(pntGrid.X + mouseOnElement.X, pntGrid.Y + mouseOnElement.Y);

				// Width: Subtract the label width
				// Height: Subtract height of menu bar and the label height
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle((int)(pntClip.X), (int)(pntClip.Y), intGridWidth - intLabelWidth, intGridHeight - intLabelHeight);
			
			} catch (Exception ex) {
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle();
				MessageBox.Show(ex.Message);
			}
		}

		private void lblMouseMove(object sender, MouseEventArgs e) {
			try {
				BibleBook lblActiveElement = sender as BibleBook;

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
				BibleBook lblActiveElement = sender as BibleBook;
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

		private bool checkLabelsTouching(object sender) {
			BibleBook lblCh = sender as BibleBook;
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
				BibleBook lbl = this.FindName(strLbl) as BibleBook;

				var tpMatchReturn = viewModel.checkTouchingLabelsCorrect(lbl, lblCh, rctChLbl, astrHebrew, astrChHebrew);
				if (tpMatchReturn.Item1) blnCorrect = true;
				if (tpMatchReturn.Item2) blnAttemptedMatch = true;

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
						viewModel.completedMatching("HebrewMatch", viewModel.propCurrentPoints, viewModel.propNumberCorrect, viewModel.propNumberAttempted);
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

				lblCh.SetBackground = Brushes.Salmon;
				viewModel.incorrectFlash(lblCh, "#E6EBF3");
			}
			else if (blnCorrect == false && blnAttemptedMatch == false) {
				// No match attempted
				return false;
			}

			// Match was attempted, whether correct or not
			return true;
		}
	}
}
