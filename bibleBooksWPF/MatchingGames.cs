using ExtensionMethods;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Markup.Localizer;
using System.Windows.Media;

namespace BibleBooksWPF {
	public class MatchingGames {
		internal static Tuple<bool, bool> checkTouchingLabelsCorrect(Label lbl, Label lblCh, Rect rctChLbl, string[] astrBooks, string[] astrChBooks) {
			Boolean blnCorrect = false;
			Boolean blnAttemptedMatch = false;

			// Turn English label into a rectangle
			Rect rctLbl = new Rect();
			rctLbl.Location = lbl.PointToScreen(new Point(0, 0));
			rctLbl.Height = lbl.ActualHeight;
			rctLbl.Width = lbl.ActualWidth;

			// Only check English labels that are touching the Chinese label
			if (rctChLbl.IntersectsWith(rctLbl) && lbl.IsEnabled) {
				int intChLabelIndex = Array.IndexOf(astrChBooks, lblCh.Name);
				blnAttemptedMatch = true;

				// If the correct English label has been matched
				if (lbl.Name == astrBooks[intChLabelIndex]) {
					// Mark boolean flag true first
					// Override the false in case it is touching 2 English labels at once
					blnCorrect = true;

					// Move correct label on top of English
					lblCh.RenderTransform = new TranslateTransform();
					Grid.SetRow(lblCh, Grid.GetRow(lbl));
					Grid.SetColumn(lblCh, Grid.GetColumn(lbl));
					lblCh.IsEnabled = false;
					lbl.IsEnabled = false;
				}
			}

			return Tuple.Create(blnCorrect, blnAttemptedMatch);
		}

		internal static string completedMatching(TimeSpan tsElapsed, int intCurrentPoints, int intNumberCorrect, int intNumberAttempted) {
			CustomMessageBox winMsgBox = new CustomMessageBox();

			// Add the game data to statistics json file
			TimeSpan time = new TimeSpan(tsElapsed.Hours, tsElapsed.Minutes, tsElapsed.Seconds);
			string strRecord = Statistics.AddStatistic("GreekMatch", intCurrentPoints, time);

			string strResponse = "";
			string strTime = String.Format("{0:00}:{1:00}:{2:00}", tsElapsed.Hours, tsElapsed.Minutes, tsElapsed.Seconds);
			if (strRecord != "") {
				// Record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intNumberAttempted) + "\n" +
							"Time Elapsed: " + strTime, "Congratulations!", "congrats", strRecord, winMsgBox);
			}
			else {
				// No record set
				strResponse = CustomMessageBoxMethods.ShowMessage("Congratulations! You have finished. Try again?\n" +
							"Percentage Correct: " + String.Format("{0:P2}", (double)intNumberCorrect / intNumberAttempted) + "\n" +
							"Time Elapsed: " + strTime, "Congratulations!", "congrats", winMsgBox);
			}

			return strResponse;
		}

	}
}
