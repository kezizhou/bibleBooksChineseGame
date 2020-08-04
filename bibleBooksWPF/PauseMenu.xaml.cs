using System.Windows;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for PauseMenu.xaml
	/// </summary>
	public partial class PauseMenu : Window {
		public PauseMenu() {
			InitializeComponent();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void btnResume_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Resume";
			this.Close();
		}

		private void btnMain_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Main";
			this.Close();
		}

		private void btnExit_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Exit";
			this.Close();
		}
	}
}
