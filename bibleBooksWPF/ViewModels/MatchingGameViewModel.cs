using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

using BibleBooksWPF.UserControls;

namespace BibleBooksWPF.ViewModels {
	public abstract class MatchingGameViewModel : BaseGameViewModel {
		internal Tuple<bool, bool> checkTouchingLabelsCorrect(BibleBook lblMatch, BibleBook lblDragged, Rect rctDragLbl, string[] astrMatchLblNames, string[] astrDragLblNames) {
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Turn label match into a rectangle
			Rect rctLblMatch = new Rect();
			rctLblMatch.Location = lblMatch.PointToScreen(new Point(0, 0));
			rctLblMatch.Height = lblMatch.ActualHeight;
			rctLblMatch.Width = lblMatch.ActualWidth;

			// Only check match labels that are touching the dragged label
			if (rctDragLbl.IntersectsWith(rctLblMatch) && lblMatch.IsEnabled) {
				int intChLabelIndex = Array.IndexOf(astrDragLblNames, lblDragged.Name);
				blnAttemptedMatch = true;

				// If the correct label has been matched
				if (lblMatch.Name == astrMatchLblNames[intChLabelIndex]) {
					// Mark boolean flag true first
					// Override the false in case it is touching 2 match labels at once
					blnCorrect = true;

					// Move correct label on top of match
					lblDragged.RenderTransform = new TranslateTransform();
					Grid.SetRow(lblDragged, Grid.GetRow(lblMatch));
					Grid.SetColumn(lblDragged, Grid.GetColumn(lblMatch));
					lblDragged.IsEnabled = false;
					lblMatch.IsEnabled = false;
				}
			}

			return Tuple.Create(blnCorrect, blnAttemptedMatch);
		}
	}
}
