using System;
using System.Windows.Forms;

namespace BibleBooks {
	public partial class Settings : Form {
		public Settings() {
			InitializeComponent();
			WindowState = FormWindowState.Maximized;

			// Add audio pronunciation settings event
			radAudioOn.CheckedChanged += new EventHandler(radAudio_CheckedChanged);
			radAudioOff.CheckedChanged += new EventHandler(radAudio_CheckedChanged);
		}

		// Audio Pronunciation Settings
		private void radAudio_CheckedChanged(object sender, EventArgs e) {
			if (radAudioOn.Checked) {
				Program.blnAudio = true;
			} else {
				Program.blnAudio = false;
			}
		}

		private void mainMenuToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			MainMenu frmMainMenu = new MainMenu();
			frmMainMenu.ShowDialog();
			this.Close();
		}

		private void hebrewScripturesToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			HebrewScriptures frmHebrew = new HebrewScriptures();
			frmHebrew.ShowDialog();
			this.Close();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekScriptures frmGreek = new GreekScriptures();
			frmGreek.ShowDialog();
			this.Close();
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}
	}
}
