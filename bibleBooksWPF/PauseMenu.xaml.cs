using System.Windows;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for PauseMenu.xaml
	/// </summary>
	public partial class PauseMenu : Window {
		public PauseMenu() {
			InitializeComponent();
		}

		public string strMsgReturn {
			get;
			set;
		}

		private void btnResume_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Resume";
			this.Close();
		}

		private void btnMain_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Main";
			this.Close();
		}

		private void btnExit_Click(object sender, RoutedEventArgs e) {
			strMsgReturn = "Exit";
			this.Close();
		}

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			winPauseMenu.Title = "游戏暂停";
			txbMessageText.Text = "游戏暂停";
			btnResume.Content = "继续";
			btnMain.Content = "回到主菜单";
			btnExit.Content = "退出";
		}
	}
}
