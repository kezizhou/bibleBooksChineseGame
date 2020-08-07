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

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
				winConfirmMessageBox.Title = "确定删除用户";
				btnYes.Content = "确定，删除";
				btnCancel.Content = "取消";
			}
		}
	}
}
