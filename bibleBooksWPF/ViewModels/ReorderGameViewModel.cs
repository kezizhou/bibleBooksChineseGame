using System;
using System.Windows;
using BibleBooksWPF.UserControls;

namespace BibleBooksWPF.ViewModels {
	public abstract class ReorderGameViewModel : BaseGameViewModel {
		internal Tuple<bool, bool> checkTouchingLabelsCorrect(BibleBook lblReorderMatch, BibleBook lblDragged, Rect rctDragLbl, string[] astrMatchLblNames, string[] astrDragLblNames) {
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Turn label match into a rectangle
			Rect rctLblMatch = new Rect();
			rctLblMatch.Location = lblReorderMatch.PointToScreen(new Point(0, 0));
			rctLblMatch.Height = lblReorderMatch.ActualHeight;
			rctLblMatch.Width = lblReorderMatch.ActualWidth;

			// Only check match labels that are touching the dragged label
			if (rctDragLbl.IntersectsWith(rctLblMatch) && lblReorderMatch.IsEnabled) {
				int intLabelIndex = Array.IndexOf(astrDragLblNames, lblDragged.Name);
				blnAttemptedMatch = true;

				// If the correct label has been matched
				if (lblReorderMatch.Name == "lbl" + (intLabelIndex + 1).ToString()) {
					// Mark boolean flag true first
					// Override the false in case it is touching 2 match labels at once
					blnCorrect = true;

					lblDragged.IsEnabled = false;
				}
			}

			return Tuple.Create(blnCorrect, blnAttemptedMatch);
		}
	}
}
