using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace BibleBooksWPF
{
    /// <summary>
    /// Interaction logic for HebrewMatch.xaml
    /// </summary>
    public partial class HebrewMatch : Page {
        public HebrewMatch() {
            InitializeComponent();
        }

		private void ImenMatchGreek_Click(object sender, RoutedEventArgs e) {
			GreekMatch pGreekMatch = new GreekMatch();
			NavigationService.Navigate(pGreekMatch);
		}

		private void ImenSettings_Click(object sender, RoutedEventArgs e) {
			Settings pSettings = new Settings();
			NavigationService.Navigate(pSettings);
		}

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}
	}
}
