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
	/// Interaction logic for GreekReorder.xaml
	/// </summary>
	public partial class GreekReorder : ContentControl {

		// Variables for moving labels
		public bool blnDragging = false;
		private Point clickPosition;
		Dictionary<string, Point> dctTransform = new Dictionary<string, Point>();

		GreekReorderViewModel viewModel;
		List<Point> lpntLabels = new List<Point>();

		public GreekReorder() {
			try {
				InitializeComponent();
				LanguageResources.SetDefaultLanguage(this);

				viewModel = this.DataContext as GreekReorderViewModel;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				Random r = new Random();

				int i = 0;
				foreach (String strLbl in viewModel.propGreek) {
					BibleBook lbl = this.FindName(strLbl) as BibleBook;

					// Check main language
					if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
						lbl.Content = viewModel.propChinese[i];
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
				foreach (String strLbl in viewModel.propGreek) {
					BibleBook lbl = this.FindName(strLbl) as BibleBook;

					// Assign the label to a random grid position
					int intRandom = r.Next(0, lpntLabels.Count);
					Grid.SetRow(lbl, (int)lpntLabels[intRandom].X);
					Grid.SetColumn(lbl, (int)lpntLabels[intRandom].Y);

					// Remove the point so it is not assigned again
					lpntLabels.Remove(new Point(Grid.GetRow(lbl), Grid.GetColumn(lbl)));
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
				int intGridWidth = Layout.TransformToPixels(grdGreekReorder, grdGreekReorder.ActualWidth);
				int intGridHeight = Layout.TransformToPixels(grdGreekReorder, grdGreekReorder.ActualHeight);

				int intLabelWidth = Layout.TransformToPixels(grdGreekReorder, lblActiveElement.ActualWidth);
				int intLabelHeight = Layout.TransformToPixels(grdGreekReorder, lblActiveElement.ActualHeight);

				Point pntGrid = grdGreekReorder.PointToScreen(grdGreekReorder.TranslatePoint(new Point(0, 0), this));
				mouseOnElement = new Point(Layout.TransformToPixels(grdGreekReorder, mouseOnElement.X), Layout.TransformToPixels(grdGreekReorder, mouseOnElement.Y));
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
					//Point mouseOnElement = Mouse.GetPosition(lblActiveElement);

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
			BibleBook lblDragged = sender as BibleBook;
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Convert the moved label into a rect
			Rect rectMovedLbl = new Rect();
			rectMovedLbl.Location = lblDragged.PointToScreen(new Point(0, 0));
			rectMovedLbl.Height = lblDragged.ActualHeight;
			rectMovedLbl.Width = lblDragged.ActualWidth;

			// Loop through the container labels to see if the moved label is touching one of them
			foreach (String strContainerName in viewModel.propReorderLbls) {
				// Get the label from the string name
				Label lblReorderMatch = (Label)this.FindName(strContainerName);

				var tpMatchReturn = viewModel.checkTouchingLabelsCorrect(lblReorderMatch, lblDragged, rectMovedLbl, viewModel.propReorderLbls, viewModel.propGreek);
				if (tpMatchReturn.Item1) blnCorrect = true;
				if (tpMatchReturn.Item2) blnAttemptedMatch = true;

				if (blnCorrect) {
					// Add points
					viewModel.AddCorrectAttempt();

					// Move label to reordered container label
					lblDragged.RenderTransform = new TranslateTransform();

					// Remove from main grid
					grdGreekReorder.Children.Remove(lblDragged);

					// Add to reordered labels grid
					grdReordered.Children.Add(lblDragged);
					Grid.SetRow(lblDragged, Grid.GetRow(lblReorderMatch));
					Grid.SetColumn(lblDragged, Grid.GetColumn(lblReorderMatch));

					viewModel.propBooksRemaining.Remove(lblDragged.Name);

					// Check if all books have been matched
					if (viewModel.propBooksRemaining.Count == 0) {
						viewModel.completedMatching("GreekReorder", viewModel.propCurrentPoints, viewModel.propNumberCorrect, viewModel.propNumberAttempted);
					}

					break;
				}
			}

			if (blnCorrect == false && blnAttemptedMatch == true) {
				// Point penalty
				viewModel.AddIncorrectAttempt();

				// Label was moved from original position
				if (dctTransform.ContainsKey(lblDragged.Name) == false) {
					lblDragged.RenderTransform = new TranslateTransform();
					dctTransform[lblDragged.Name] = new Point(0, 0);
				} else {
					// Return to previous position before match
					TranslateTransform transform = lblDragged.RenderTransform as TranslateTransform;
					transform.X = dctTransform[lblDragged.Name].X;
					transform.Y = dctTransform[lblDragged.Name].Y;
				}

				lblDragged.SetBackground = Brushes.Salmon;
				viewModel.incorrectFlash(lblDragged);
			} else if (blnCorrect == false && blnAttemptedMatch == false) {
				// No match attempted
				return false;
			}

			// Match was attempted, whether correct or not
			return true;
		}
	}
}
