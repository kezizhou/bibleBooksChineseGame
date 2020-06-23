﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Speech.Synthesis;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BibleBooks {
	public partial class HebrewScriptures : Form {
		private Control activeControl;
		private Point previousLocation;
		private Point locationBeforeMatch;
		private static int intHebrewAnswered = 0;
		private static int intNumberCorrect = 0;
		private static int intHebrewPoints = 0;
		private static TimeSpan tsSecondsElapsed = TimeSpan.FromSeconds(0);
		List<Point> lpntChLabels = new List<Point>();

		String[] astrChHebrew = new String[] { "lblChGenesis", "lblChExodus", "lblChLeviticus", "lblChNumbers", "lblChDeuteronomy", "lblChJoshua", "lblChJudges", "lblChRuth", "lblCh1Samuel",
											"lblCh2Samuel", "lblCh1Kings", "lblCh2Kings", "lblCh1Chronicles", "lblCh2Chronicles", "lblChEzra", "lblChNehemiah", "lblChEsther", "lblChJob",
											"lblChPsalms", "lblChProverbs", "lblChEcclesiastes", "lblChSongofSolomon", "lblChIsaiah", "lblChJeremiah", "lblChLamentations", "lblChEzekiel",
											"lblChDaniel", "lblChHosea", "lblChJoel", "lblChAmos", "lblChObadiah", "lblChJonah", "lblChMicah", "lblChNahum", "lblChHabakkuk", "lblChZephaniah",
											"lblChHaggai", "lblChZechariah", "lblChMalachi"};

		String[] astrHebrew = new String[] { "lblGenesis", "lblExodus", "lblLeviticus", "lblNumbers", "lblDeuteronomy", "lblJoshua", "lblJudges", "lblRuth", "lbl1Samuel",
											"lbl2Samuel", "lbl1Kings", "lbl2Kings", "lbl1Chronicles", "lbl2Chronicles", "lblEzra", "lblNehemiah", "lblEsther", "lblJob",
											"lblPsalms", "lblProverbs", "lblEcclesiastes", "lblSongofSolomon", "lblIsaiah", "lblJeremiah", "lblLamentations", "lblEzekiel",
											"lblDaniel", "lblHosea", "lblJoel", "lblAmos", "lblObadiah", "lblJonah", "lblMicah", "lblNahum", "lblHabakkuk", "lblZephaniah",
											"lblHaggai", "lblZechariah", "lblMalachi"};

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

			// Center panel
			pnlWindowResize.Left = (this.ClientSize.Width - pnlWindowResize.Width) / 2;
			pnlWindowResize.Top = (this.ClientSize.Height - pnlWindowResize.Height) / 2;
		}

		private void lblMouseDown(object sender, MouseEventArgs e) {
			activeControl = sender as Control;
			locationBeforeMatch = activeControl.Location;
			previousLocation = e.Location;
			Cursor = Cursors.Hand;
			activeControl.BringToFront();

			// Check audio setting
			// If on, play audio
			if (Program.blnAudio) {
				// Use a task so mouse move event will not wait on audio
				Task.Run(() => playChineseAudio(sender));
			}
		}

		private void lblMouseMove(object sender, MouseEventArgs e) {
			if (activeControl == null || activeControl != sender)
				return;

			// Have control follow mouose drag
			var location = activeControl.Location;
			location.Offset(e.Location.X - previousLocation.X, e.Location.Y - previousLocation.Y);
			activeControl.Location = location;
		}

		private void lblMouseUp(object sender, MouseEventArgs e) {
			activeControl = null;
			Cursor = Cursors.Default;

			// Check if it has been matched to an English book
			checkLabelsTouching(sender);
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
			Label lblCh = sender as Label;
			Boolean blnCorrect = false;

			// Check each English book to see if touching
			foreach (String strLbl in astrHebrew) {
				// Get the label from the string name
				Label lbl = this.Controls.Find(strLbl, true).FirstOrDefault() as Label;

				// Only check English labels that are touching the Chinese label
				if ( lblCh.Bounds.IntersectsWith(lbl.Bounds) ) {

					int intChLabelIndex = Array.IndexOf(astrChHebrew, lblCh.Name);

					// If the correct English label has been matched
					if (strLbl == astrHebrew[intChLabelIndex]) {
						// Mark boolean flag true first
						// Override the false in case it is touching 2 English labels at once
						blnCorrect = true;

						// Add points
						intHebrewPoints += 1;
						Program.intTotalPoints += 1;
						intNumberCorrect += 1;
						intHebrewAnswered += 1;
						refreshPoints();
						lblCh.Location = lbl.Location;
						lblCh.Enabled = false;
						lbl.Hide();

					}
				}
			}

			if (blnCorrect == false) {
				// Point penalty
				intHebrewPoints -= 1;
				Program.intTotalPoints -= 1;
				intHebrewAnswered += 1;
				refreshPoints();
				lblCh.Location = locationBeforeMatch;
				lblCh.BackColor = Color.Salmon;
				incorrectFlash(lblCh);
			}
		}

		private void refreshPoints() {
			lblHebrewPoints.Text = intHebrewPoints.ToString();
			lblTotalPoints.Text = Program.intTotalPoints.ToString();
			lblPercentageCorrect.Text = ((double)intNumberCorrect / intHebrewAnswered * 100).ToString();
			lblHebrewAnswered.Text = intHebrewAnswered.ToString();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			MainMenu frmMainMenu = new MainMenu();
			frmMainMenu.Show();
			this.Close();
		}

		private void timer1_Tick(object sender, EventArgs e) {
			tsSecondsElapsed += TimeSpan.FromSeconds(1);
			lblTimeElapsed.Text = tsSecondsElapsed.ToString();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			Settings frmSettings = new Settings();
			frmSettings.Show();
			this.Close();
		}

		private void matchChineseToEnglishToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekScriptures frmGreek = new GreekScriptures();
			frmGreek.Show();
			this.Close();
		}

		private void reorderBooksToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekReorder frmGreekReorder = new GreekReorder();
			frmGreekReorder.Show();
			this.Close();
		}

		private async void incorrectFlash(Label lblIncorrectBook) {
			await Task.Delay(900);
			lblIncorrectBook.BackColor = Color.Azure;
		}
	}
}
