using System;
using System.Windows.Forms;

namespace BibleBooks {
	public partial class MainMenu : Form {

		public MainMenu() {
			InitializeComponent();
		}

		private void greekScripturesToolStripMenuItem_Click(object sender, EventArgs e) {
			this.Hide();
			GreekScriptures frmGreek = new GreekScriptures();
			frmGreek.ShowDialog();
			this.Close();
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
	}
}