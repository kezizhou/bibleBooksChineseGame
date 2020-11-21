using System.Windows.Controls;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for DiceBooksGreek.xaml
	/// </summary>
	public partial class DiceBooksGreek : ContentControl {
		public DiceBooksGreek() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
		}
	}
}
