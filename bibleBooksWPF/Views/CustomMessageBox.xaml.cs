using System.Windows;
using System.Windows.Media;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for CustomMessageBox.xaml
	/// </summary>
	public partial class CustomMessageBox : Window {
		public CustomMessageBox() {
			InitializeComponent();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void BtnMain_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Main";
			this.Close();
		}

		private void BtnRetry_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Retry";
			this.Close();
		}

		private void BtnExit_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Exit";
			this.Close();
		}
	}
}
