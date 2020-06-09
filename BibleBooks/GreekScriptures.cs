using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Speech.Synthesis;
using System.Globalization;
using System.Collections.Generic;

namespace BibleBooks {
	public partial class GreekScriptures : Form {

		private Control activeControl;
		private Point previousLocation;
		private static int intGreekAnswered = 0;
		private static int intNumberCorrect = 0;
		private static TimeSpan tsSecondsElapsed = TimeSpan.FromSeconds(0);
		List<Point> lpntChLabels = new List<Point>();

		String[] astrChGreek = new String[] { "lblChMatthew", "lblChMark", "lblChLuke", "lblChJohn", "lblChActs", "lblChRomans", "lblCh1Corinthians", "lblCh2Corinthians", "lblChGalatians",
											"lblChEphesians", "lblChPhilippians", "lblChColossians", "lblCh1Thessalonians", "lblCh2Thessalonians", "lblCh1Timothy", "lblCh2Timothy", "lblChTitus",
											"lblChPhilemon", "lblChHebrews", "lblChJames", "lblCh1Peter", "lblCh2Peter", "lblCh1John", "lblCh2John", "lblCh3John", "lblChRevelation" };

		String[] astrGreek = new String[] { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
											"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
											"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lblCh3John", "lblChRevelation" };

		public GreekScriptures() {
			InitializeComponent();
			WindowState = FormWindowState.Maximized;
		}

		private void GreekScriptures_Load(object sender, EventArgs e) {
			Random r = new Random();

			foreach (String strChLbl in astrChGreek) {
				// Add draggable label methods
				Label lbl = this.Controls.Find(strChLbl, true).FirstOrDefault() as Label;
				lbl.MouseDown += new MouseEventHandler(lblMouseDown);
				lbl.MouseMove += new MouseEventHandler(lblMouseMove);
				lbl.MouseUp += new MouseEventHandler(lblMouseUp);

				// Add each label's location to a list of points
				lpntChLabels.Add(lbl.Location);
			}

			// Randomly shuffle all Chinese label locations
			foreach (String strChLbl in astrChGreek) {
				Label lbl = this.Controls.Find(strChLbl, true).FirstOrDefault() as Label;
				lbl.Location = lpntChLabels[r.Next(0, lpntChLabels.Count)];
				lpntChLabels.Remove(lbl.Location);
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

			// Check if it has been matched to an English book
			checkLabelsTouching(sender);

			// Play audio
			Label lblChineseBook = sender as Label;
			var synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("zh-CN"));
			synthesizer.Speak(lblChineseBook.Text);
		}

		private void checkLabelsTouching(object sender) {
			int i = 0;
			Label lblCh = sender as Label;

			// Check each English book to see if touching
			foreach (String strLbl in astrGreek) {
				// Get the label from the string name
				Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;

				// Only check labels that have not been correctly matched already
				if (lblCh.Enabled) {

					// Label is touching an English label
					if (lblCh.Bounds.IntersectsWith(lbl.Bounds)) {

						// If the correct English label has been matched
						if (strLbl == astrGreek[i]) {
							Program.intGreekPoints += 1;
							Program.intTotalPoints += 1;
							intNumberCorrect += 1;
							intGreekAnswered += 1;
							refreshPoints();
							lblCh.Location = lbl.Location;
							lblCh.Enabled = false;
							lbl.Hide();
						}
						else {
							// Point penalty
							Program.intGreekPoints -= 1;
							Program.intTotalPoints -= 1;
							intGreekAnswered += 1;
							refreshPoints();
							lblCh.Location = new Point(283, 305);
						}
					}
				}
				i += 1;
			}
		}

		private void refreshPoints() {
			lblGreekPoints.Text = Program.intGreekPoints.ToString();
			lblTotalPoints.Text = Program.intTotalPoints.ToString();
			lblPercentageCorrect.Text = ( (double)intNumberCorrect / intGreekAnswered * 100 ).ToString();
			lblGreekAnswered.Text = intGreekAnswered.ToString();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void hebrewScripturesToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			HebrewScriptures frmHebrew = new HebrewScriptures();
			frmHebrew.ShowDialog();
			this.Close();
		}

		private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			MainMenu frmMainMenu = new MainMenu();
			frmMainMenu.ShowDialog();
			this.Close();
		}

		private void ChineseBook_Click(object sender, EventArgs e) {
			Label lblChineseBook = sender as Label;
			var synthesizer = new SpeechSynthesizer();
			synthesizer.SetOutputToDefaultAudioDevice();
			synthesizer.SelectVoiceByHints(VoiceGender.Neutral, VoiceAge.NotSet, 0, CultureInfo.GetCultureInfo("zh-CN"));
			synthesizer.Speak(lblChineseBook.Text);
		}

		private void timer1_Tick(object sender, EventArgs e) {
			tsSecondsElapsed += TimeSpan.FromSeconds(1);
			lblTimeElapsed.Text = tsSecondsElapsed.ToString();
		}
	}
}