using System;
using System.Windows.Forms;

namespace BibleBooks {
	public partial class MainMenu : Form {

		public MainMenu() {
			InitializeComponent();
			WindowState = FormWindowState.Maximized;
		}

		private void exitToolStripMenuItem_Click(object sender, EventArgs e) {
			Close();
		}

		private void settingsToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			Settings frmSettings = new Settings();
			frmSettings.ShowDialog();
			this.Close();
		}

		private void reorderBooksToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekReorder frmGreekReorder = new GreekReorder();
			frmGreekReorder.ShowDialog();
			this.Close();
		}

		private void matchChineseToEnglishHebrewToolStripMenuItem1_Click(object sender, EventArgs e) {
			this.Hide();
			HebrewScriptures frmHebrew = new HebrewScriptures();
			frmHebrew.ShowDialog();
			this.Close();
		}

		private void matchChineseToEnglishGreekToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekScriptures frmGreek = new GreekScriptures();
			frmGreek.ShowDialog();
			this.Close();
		}
	}
}