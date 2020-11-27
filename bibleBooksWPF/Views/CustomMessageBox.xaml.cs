using BibleBooksWPF.ViewModels;
using System.Windows;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for CustomMessageBox.xaml
	/// </summary>
	public partial class CustomMessageBox : Window {
		CustomMessageBoxViewModel viewModel;
		public bool blnButtonClicked = false;

		public CustomMessageBox() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
			viewModel = this.DataContext as CustomMessageBoxViewModel;
		}

		public CustomMessageBox(string strMessage, string strRecord) {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
			viewModel = this.DataContext as CustomMessageBoxViewModel;

			viewModel.propMessage = strMessage;
			viewModel.propRecord = strRecord;

			this.ShowDialog();
		}

		public string strMsgReturn { get; set; }

		private void BtnMain_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Main";
			blnButtonClicked = true;
			this.Close();
		}

		private void BtnRetry_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Retry";
			blnButtonClicked = true;
			this.Close();
		}

		private void BtnExit_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Exit";
			blnButtonClicked = true;
			this.Close();
		}
	}
}
