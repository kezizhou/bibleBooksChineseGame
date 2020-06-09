using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Windows.Forms;

namespace BibleBooks {
	public partial class HebrewScriptures : Form {
		private Control activeControl;
		private Point previousLocation;
		private static int intHebrewAnswered = 0;
		private static int intNumberCorrect = 0;
		private static TimeSpan tsSecondsElapsed = TimeSpan.FromSeconds(0);
		List<Point> lpntChLabels = new List<Point>();

		String[] astrChHebrew = new String[] { "lblChGenesis", "lblChExodus", "lblChLeviticus", "lblChNumbers", "lblChDeuteronomy", "lblChJoshua", "lblChJudges", "lblChRuth", "lblCh1Samuel",
											"lblCh2Samuel", "lblCh1Kings", "lblCh2Kings", "lblCh1Chronicles", "lblCh2Chronicles", "lblChEzra", "lblChNehemiah", "lblChEsther", "lblChJob",
											"lblChPsalms", "lblChProverbs", "lblChEcclesiastes", "lblChSongofSolomon", "lblChIsaiah", "lblChJeremiah", "lblChLamentations", "lblChEzekiel"};

		String[] astrHebrew = new String[] { "lblGenesis", "lblExodus", "lblLeviticus", "lblNumbers", "lblDeuteronomy", "lblJoshua", "lblJudges", "lblRuth", "lbl1Samuel",
											"lbl2Samuel", "lbl1Kings", "lbl2Kings", "lbl1Chronicles", "lbl2Chronicles", "lblEzra", "lblNehemiah", "lblEsther", "lblJob",
											"lblPsalms", "lblProverbs", "lblEcclesiastes", "lblSongofSolomon", "lblIsaiah", "lblJeremiah", "lblLamentations", "lblEzekiel" };

		public HebrewScriptures() {
			InitializeComponent();
			WindowState = FormWindowState.Maximized;
		}

		private void HebrewScriptures_Load(object sender, EventArgs e) {
			Random r = new Random();

			foreach (String strLbl in astrChHebrew) {
				// Add draggable label methods
				Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;
				lbl.MouseDown += new MouseEventHandler(lblMouseDown);
				lbl.MouseMove += new MouseEventHandler(lblMouseMove);
				lbl.MouseUp += new MouseEventHandler(lblMouseUp);

				// Add each label's location to a list of points
				lpntChLabels.Add(lbl.Location);
			}

			// Randomly shuffle all Chinese label locations
			foreach (String strChLbl in astrChHebrew) {
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

			// Check audio setting
			// If on, play audio
			if (Program.blnAudio) {
				playChineseAudio(sender);
			}

		}

		private void playChineseAudio(object sender) {
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
			foreach (String strLbl in astrHebrew) {
				// Get the label from the string name
				Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;

				// Only check labels that have not been correctly matched already
				if (lblCh.Enabled) {

					// Label is touching an English label
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
						} else {
							// Point penalty
							Program.intHebrewPoints -= 1;
							Program.intTotalPoints -= 1;
							intHebrewAnswered += 1;
							refreshPoints();
							lblCh.Location = new Point(283, 305);
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

		private void timer1_Tick(object sender, EventArgs e) {
			tsSecondsElapsed += TimeSpan.FromSeconds(1);
			lblTimeElapsed.Text = tsSecondsElapsed.ToString();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			Settings frmSettings = new Settings();
			frmSettings.ShowDialog();
			this.Close();
		}
	}
}
