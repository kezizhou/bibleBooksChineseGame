using System.Windows;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for NewBadgeMessage.xaml
	/// </summary>
	public partial class NewBadgeMessage : Window {
		public NewBadgeMessage() {
			InitializeComponent();
		}

		private void BtnOK_Click(object sender, RoutedEventArgs e) {
			this.Close();
		}
	}
}
