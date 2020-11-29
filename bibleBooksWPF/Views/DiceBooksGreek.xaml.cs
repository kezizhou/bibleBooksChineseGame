using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;

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
