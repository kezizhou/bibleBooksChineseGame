using System.Windows;

namespace BibleBooksWPF.Views {
    /// <summary>
    /// Interaction logic for NewUser.xaml
    /// </summary>
    public partial class NewUser : Window {
		public NewUser() {
            InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
        }

		public string strMsgReturn {
			get;
			set;
		}

		private void BtnOK_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "";
			this.Close();
		}

		private void BtnCancel_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Cancel";
			this.Close();
		}
	}
}
