using System;
using System.Windows.Forms;

namespace BibleBooks {
	public partial class CustomMessageBox : Form {
		public CustomMessageBox() {
			InitializeComponent();
		}

		private void btnRetry_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.Retry;
			this.Close();
		}

		private void btnMain_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.OK;
			this.Close();
		}

		private void btnExit_Click(object sender, EventArgs e) {
			this.DialogResult = DialogResult.Abort;
			this.Close();
		}

	}
}
