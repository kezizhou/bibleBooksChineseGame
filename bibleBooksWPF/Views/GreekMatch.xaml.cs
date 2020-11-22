using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

using BibleBooksWPF.Helpers;
using BibleBooksWPF.ViewModels;
using BibleBooksWPF.UserControls;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for GreekMatch.xaml
	/// </summary>
	public partial class GreekMatch : ContentControl {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		GreekMatchViewModel viewModel;
		Dictionary<string, Point> dctTransform = new Dictionary<String, Point>();

		List<Point> lpntChLabels = new List<Point>();

		static string[] astrChGreek = { "lblChMatthew", "lblChMark", "lblChLuke", "lblChJohn", "lblChActs", "lblChRomans", "lblCh1Corinthians", "lblCh2Corinthians", "lblChGalatians",
									"lblChEphesians", "lblChPhilippians", "lblChColossians", "lblCh1Thessalonians", "lblCh2Thessalonians", "lblCh1Timothy", "lblCh2Timothy", "lblChTitus",
									"lblChPhilemon", "lblChHebrews", "lblChJames", "lblCh1Peter", "lblCh2Peter", "lblCh1John", "lblCh2John", "lblCh3John", "lblChJude", "lblChRevelation" };

		static string[] astrGreek = { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
									"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
									"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lbl3John", "lblJude", "lblRevelation" };

		List<String> lstrBooksToComplete = new List<String>(astrChGreek);

		public GreekMatch() {
			try {
				InitializeComponent();
				LanguageResources.SetDefaultLanguage(this);

				viewModel = this.DataContext as GreekMatchViewModel;

				//Messenger.Default.Register<NotificationMessageAction<Label, bool>>(this,
				//	(message) => {
				//		Label lblBook = message.Content;

				//		switch (message.Notification) {
				//			case "CheckTouching":
				//				checkLabelsTouching(lblBook);
				//				message.Execute(true);
				//				break;
				//			default:
				//				break;
				//	}
				//});
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				Random r = new Random();

				foreach (String strChLbl in astrChGreek) {
					BibleBook lblCh = this.FindName(strChLbl) as BibleBook;

					// Check main language
					if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
						string strTemp = lblCh.Content.ToString();

						BibleBook lblEn = this.FindName(strChLbl.Remove(3, 2)) as BibleBook;
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
					BibleBook lblCh = this.FindName(strChLbl) as BibleBook;

					// Assign the label to a random grid position
					int intRandom = r.Next(0, lpntChLabels.Count);
					Grid.SetRow(lblCh, (int)lpntChLabels[intRandom].X);
					Grid.SetColumn(lblCh, (int)lpntChLabels[intRandom].Y);

					// Remove the point so it is not assigned again
					lpntChLabels.Remove(new Point(Grid.GetRow(lblCh), Grid.GetColumn(lblCh)));
				}
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
				int intGridWidth = Layout.TransformToPixels(grdGreekMatch, grdGreekMatch.ActualWidth);
				int intGridHeight = Layout.TransformToPixels(grdGreekMatch, grdGreekMatch.ActualHeight);

				int intLabelWidth = Layout.TransformToPixels(grdGreekMatch, lblActiveElement.ActualWidth);
				int intLabelHeight = Layout.TransformToPixels(grdGreekMatch, lblActiveElement.ActualHeight);

				int intMenuWidth = Layout.TransformToPixels(grdGreekMatch, ((MainWindow)App.Current.MainWindow).menTop.ActualWidth);
				int intMenuHeight = Layout.TransformToPixels(grdGreekMatch, ((MainWindow)App.Current.MainWindow).menTop.ActualHeight);

				Point pntGrid = grdGreekMatch.PointToScreen(grdGreekMatch.TranslatePoint(new Point(0, 0), this));
				mouseOnElement = new Point(Layout.TransformToPixels(grdGreekMatch, mouseOnElement.X), Layout.TransformToPixels(grdGreekMatch, mouseOnElement.Y));
				Point pntClip = new Point(pntGrid.X + mouseOnElement.X, pntGrid.Y + mouseOnElement.Y + intMenuHeight);

				// Width: Subtract the label width
				// Height: Subtract height of menu bar and the label height
				System.Windows.Forms.Cursor.Clip = new System.Drawing.Rectangle((int)(pntClip.X), (int)(pntClip.Y), intGridWidth - intLabelWidth, intGridHeight - intMenuHeight - intLabelHeight);
				
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
				BibleBook lblActiveElement = sender as BibleBook;
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

		private bool checkLabelsTouching(object sender) {
			BibleBook lblCh = sender as BibleBook;
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
				BibleBook lbl = this.FindName(strLbl) as BibleBook;

				var tpMatchReturn = viewModel.checkTouchingLabelsCorrect(lbl, lblCh, rctChLbl, astrGreek, astrChGreek);
				if (tpMatchReturn.Item1) blnCorrect = true;
				if (tpMatchReturn.Item2) blnAttemptedMatch = true;

				if (blnCorrect) {
					// Add points
					viewModel.AddCorrectAttempt();

					lstrBooksToComplete.Remove(lblCh.Name);

					// Finished matching
					if (lstrBooksToComplete.Count == 0) {
						viewModel.completedMatching("GreekMatch", viewModel.propCurrentPoints, viewModel.propNumberCorrect, viewModel.propNumberAttempted);
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

				lblCh.SetBackground = Brushes.Salmon;
				viewModel.incorrectFlash(lblCh);
			} else if(blnCorrect == false && blnAttemptedMatch == false) {
				// No match attempted
				return false;
			}

			// Match was attempted, whether correct or not
			return true;
		}
	}
}
