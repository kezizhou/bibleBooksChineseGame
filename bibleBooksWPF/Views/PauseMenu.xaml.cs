using System.Windows;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for PauseMenu.xaml
	/// </summary>
	public partial class PauseMenu : Window {
		public bool blnButtonClicked = false;
		public PauseMenu() {
			InitializeComponent();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void btnResume_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Resume";
			blnButtonClicked = true;
			this.Close();
		}

		private void btnMain_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Main";
			blnButtonClicked = true;
			this.Close();
		}

		private void btnExit_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Exit";
			blnButtonClicked = true;
			this.Close();
		}

		private void winPauseMenu_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (!blnButtonClicked) {
				strMsgReturn = "Resume";
			}
		}
	}
}
