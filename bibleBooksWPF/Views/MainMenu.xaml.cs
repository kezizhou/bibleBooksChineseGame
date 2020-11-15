using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using BibleBooksWPF.Resources;
using BibleBooksWPF.Helpers;
using GalaSoft.MvvmLight.Messaging;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class MainMenu : ContentControl {

		public MainMenu() {
			try {
				InitializeComponent();
				LanguageResources.SetDefaultLanguage(this);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
					lblWelcome.Content = "欢迎: " + App.Current.Properties["currentUsername"];
				} else if (Properties.Settings.Default.strLanguage.Equals("en-US")) {
					lblWelcome.Content = "Welcome:  " + App.Current.Properties["currentUsername"];
				}
				
				List<string> lstBadges = StatisticMethods.GetBadges();
				NewBadgeMessage winBadgeBox = new NewBadgeMessage();

				TimeSpan tsMorning = new TimeSpan(8, 30, 0);
				TimeSpan tsNight = new TimeSpan(21, 0, 0);
				if (DateTime.Now.TimeOfDay < tsMorning && !lstBadges.Contains("imgBadgeMorning")) {
					// Morning badge earned
					StatisticMethods.AddBadge("imgBadgeMorning");
					CustomMessageBoxMethods.ShowMessage("imgBadgeMorning", winBadgeBox);
				} else if (DateTime.Now.TimeOfDay > tsNight && !lstBadges.Contains("imgBadgeNight")) {
					// Night badge earned
					StatisticMethods.AddBadge("imgBadgeNight");
					CustomMessageBoxMethods.ShowMessage("imgBadgeNight", winBadgeBox);
				}
				App.Current.Properties["exodusBadge"] = "";
				App.Current.Properties["ruthBadge"] = "";
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnChangeUser_Click(object sender, RoutedEventArgs e) {
			try {
				SelectUser pSelectUser = new SelectUser();
				var navMessage = new NotificationMessage("SelectUser");
				Messenger.Default.Send(navMessage);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void btnHelp_Click(object sender, RoutedEventArgs e) {
			try {
				HelpWindow helpWindow = new HelpWindow();
				helpWindow.ShowDialog();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
