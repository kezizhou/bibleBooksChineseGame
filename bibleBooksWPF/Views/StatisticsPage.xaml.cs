using System.Windows.Controls;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for Statistics.xaml
	/// </summary>
	public partial class StatisticsPage : ContentControl {
        public StatisticsPage() {
            InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
		}
	}
}
