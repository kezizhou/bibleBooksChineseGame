using System.Windows;


namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for ConfirmMessageBox.xaml
	/// </summary>
	public partial class ConfirmMessageBox : Window {
		public ConfirmMessageBox() {
			InitializeComponent();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void btnYes_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Yes";
			this.Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Cancel";
			this.Close();
		}
	}
}
