using System.Linq;
using System.Windows.Forms;

namespace BibleBooks {
	public static class CustomMessageBoxMethods {
		public static DialogResult ShowMessage(string strMessage, string strCaption, string icon, CustomMessageBox frmMsgBox) {
			Label lbl = frmMsgBox.Controls.Find("lblMessageText", true).FirstOrDefault() as Label;
			PictureBox pic = frmMsgBox.Controls.Find("picIcon", true).FirstOrDefault() as PictureBox;

			lbl.Text = strMessage;

			switch (icon) {
				// Icon made by Flat Icons from www.flaticon.com
				case "congrats":
					pic.BackgroundImage = Properties.Resources.congrats;
					break;
				default:
					break;
			}

			frmMsgBox.StartPosition = FormStartPosition.CenterParent;
			return frmMsgBox.ShowDialog();
		}
	}
}
