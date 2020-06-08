using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibleBooks {
	public partial class HebrewScriptures : Form {
		private Control activeControl;
		private Point previousLocation;
		private static int intHebrewAnswered = 0;
		private static int intNumberCorrect = 0;

		String[] astrChHebrew = new String[] { "lblChGenesis", "lblChExodus", "lblChLeviticus", "lblChNumbers", "lblChDeuteronomy", "lblChJoshua", "lblChJudges", "lblChRuth", "lblCh1Samuel",
											"lblCh2Samuel", "lblCh1Kings", "lblCh2Kings", "lblCh1Chronicles" };

		String[] astrHebrew = new String[] { "lblGenesis", "lblExodus", "lblLeviticus", "lblNumbers", "lblDeuteronomy", "lblJoshua", "lblJudges", "lblRuth", "lbl1Samuel",
											"lbl2Samuel", "lbl1Kings", "lbl2Kings", "lbl1Chronicles" };

		public HebrewScriptures() {
			InitializeComponent();
		}

		private void HebrewScriptures_Load(object sender, EventArgs e) {
			foreach (String strLbl in astrChHebrew) {
				Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;
				lbl.MouseDown += new MouseEventHandler(lblMouseDown);
				lbl.MouseMove += new MouseEventHandler(lblMouseMove);
				lbl.MouseUp += new MouseEventHandler(lblMouseUp);
			}
		}

		private void lblMouseDown(object sender, MouseEventArgs e) {
			activeControl = sender as Control;
			previousLocation = e.Location;
			Cursor = Cursors.Hand;
			activeControl.BringToFront();
		}

		private void lblMouseMove(object sender, MouseEventArgs e) {
			if (activeControl == null || activeControl != sender)
				return;

			var location = activeControl.Location;
			location.Offset(e.Location.X - previousLocation.X, e.Location.Y - previousLocation.Y);
			activeControl.Location = location;
		}

		private void lblMouseUp(object sender, MouseEventArgs e) {
			activeControl = null;
			Cursor = Cursors.Default;
			checkLabelsTouching();
		}

		private void checkLabelsTouching() {
			int i = 0;

			foreach (String strLblCh in astrChHebrew) {
				foreach (String strLbl in astrHebrew) {
					// Get the label with the string name
					Label lblCh = this.Controls.Find(strLblCh, true).FirstOrDefault() as Label;
					Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;

					// Only check labels that have not been completed
					if (lblCh.Enabled) {

						// Chinese label is touching an English label
						if (lblCh.Bounds.IntersectsWith(lbl.Bounds)) {

							// If the correct English label has been matched
							if (strLbl == astrHebrew[i]) {
								Program.intHebrewPoints += 1;
								Program.intTotalPoints += 1;
								intNumberCorrect += 1;
								intHebrewAnswered += 1;
								refreshPoints();
								lblCh.Location = lbl.Location;
								lblCh.Enabled = false;
								lbl.Hide();
							}
							else {
								// Point penalty
								Program.intHebrewPoints -= 1;
								Program.intTotalPoints -= 1;
								intHebrewAnswered += 1;
								refreshPoints();
								lblCh.Location = new Point(283, 305);
							}
							break;
						}
					}
				}
				i += 1;
			}
		}

		private void refreshPoints() {
			lblHebrewPoints.Text = Program.intHebrewPoints.ToString();
			lblTotalPoints.Text = Program.intTotalPoints.ToString();
			lblPercentageCorrect.Text = ((double)intNumberCorrect / intHebrewAnswered * 100).ToString();
			lblHebrewAnswered.Text = intHebrewAnswered.ToString();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void greekScripturesToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekScriptures frmGreek = new GreekScriptures();
			frmGreek.ShowDialog();
			this.Close();
		}

		private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			MainMenu frmMainMenu = new MainMenu();
			frmMainMenu.ShowDialog();
			this.Close();
		}
	}
}
