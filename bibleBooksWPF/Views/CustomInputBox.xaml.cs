using System.Windows;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for CustomInputBox.xaml
	/// </summary>
	public partial class CustomInputBox : Window {
		public CustomInputBox() {
			InitializeComponent();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void BtnOK_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "";
			this.Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Cancel";
			this.Close();
		}
	}
}
