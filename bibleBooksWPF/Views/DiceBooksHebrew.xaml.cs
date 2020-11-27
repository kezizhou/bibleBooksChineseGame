using System.Windows.Controls;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for DiceBooksHebrew.xaml
	/// </summary>
	public partial class DiceBooksHebrew : ContentControl {
		public DiceBooksHebrew() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
		}
	}
}
