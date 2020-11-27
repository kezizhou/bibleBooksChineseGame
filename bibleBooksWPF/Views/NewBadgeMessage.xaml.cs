using BibleBooksWPF.ViewModels;
using System.Windows;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for NewBadgeMessage.xaml
	/// </summary>
	public partial class NewBadgeMessage : Window {
		NewBadgeMessageViewModel viewModel;
		public NewBadgeMessage() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
			viewModel = this.DataContext as NewBadgeMessageViewModel;
		}

		public NewBadgeMessage(string strBadgeName) {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
			viewModel = this.DataContext as NewBadgeMessageViewModel;

			viewModel.propBadgeName = strBadgeName;
			this.ShowDialog();
		}

		private void BtnOK_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}
}
