﻿using System.Windows;

namespace BibleBooksWPF.Views {
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

		private void Window_Loaded(object sender, RoutedEventArgs e) {
			if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
				winNewBadgeMessage.Title = "新的奖章！";
				txbMessageText.Text = "祝贺你得到这个新的奖章！";
				txbDescription.Text = "说明: ";
			}
		}
	}
}
