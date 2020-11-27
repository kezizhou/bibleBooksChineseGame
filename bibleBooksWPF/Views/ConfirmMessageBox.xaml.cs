using BibleBooksWPF.ViewModels;
using System.Windows;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for ConfirmMessageBox.xaml
	/// </summary>
	public partial class ConfirmMessageBox : Window {
		ConfirmMessageBoxViewModel viewModel;
		public bool blnButtonClicked = false;

		public ConfirmMessageBox() {
			InitializeComponent();
			viewModel = this.DataContext as ConfirmMessageBoxViewModel;
		}

		public ConfirmMessageBox(string strMessage) {
			InitializeComponent();
			viewModel = this.DataContext as ConfirmMessageBoxViewModel;
			viewModel.propMessage = strMessage;
			this.ShowDialog();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void btnYes_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Yes";
			blnButtonClicked = true;
			this.Close();
		}

		private void btnCancel_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Cancel";
			blnButtonClicked = true;
			this.Close();
		}

		private void winConfirmMessageBox_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
			if (!blnButtonClicked) {
				strMsgReturn = "Cancel";
			}
		}
	}
}
