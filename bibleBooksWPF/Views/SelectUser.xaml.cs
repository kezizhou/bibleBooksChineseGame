using BibleBooksWPF.Helpers;
using GalaSoft.MvvmLight.Messaging;
using System.Windows.Controls;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for SelectUser.xaml
	/// </summary>
	public partial class SelectUser : ContentControl {
		public SelectUser() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
			Messenger.Default.Register<RefreshPageMessage>(this, (message) => RefreshPage(message));
		}

		private void RefreshPage(RefreshPageMessage message) {
			LanguageResources.SetDefaultLanguage(this);
		}
	}
}
