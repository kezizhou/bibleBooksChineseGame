﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using ExtensionMethods;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class MainMenu : Page {

		public MainMenu() {
			try {
				InitializeComponent();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					imenMain.Header = "主菜单";
					imenHebrew.Header = "希伯来语经卷";
					imenMatchHebrew.Header = "中英文配对";
					imenReorderHebrew.Header = "排序";
					imenGreek.Header = "希腊语经卷";
					imenMatchGreek.Header = "中英文配对";
					imenReorderGreek.Header = "排序";
					imenStatistics.Header = "成绩";
					imenSettings.Header = "设置";
					imenExit.Header = "退出";
					lblWelcome.Content = "欢迎: " + App.Current.Properties["currentUsername"];
					txbDescription.Text = "点击以上的链接选择游戏";
					menTop.FontSize = 16;
				} else if (Properties.Settings.Default.strLanguage.Equals("English")) {
					lblWelcome.Content = "Welcome:  " + App.Current.Properties["currentUsername"];
				}
				
				List<string> lstBadges = Statistics.GetBadges();
				NewBadgeMessage winBadgeBox = new NewBadgeMessage();

				TimeSpan tsMorning = new TimeSpan(8, 30, 0);
				TimeSpan tsNight = new TimeSpan(21, 0, 0);
				if (DateTime.Now.TimeOfDay < tsMorning && !lstBadges.Contains("imgBadgeMorning")) {
					// Morning badge earned
					Statistics.AddBadge("imgBadgeMorning");
					CustomMessageBoxMethods.ShowMessage("imgBadgeMorning", winBadgeBox);
				} else if (DateTime.Now.TimeOfDay > tsNight && !lstBadges.Contains("imgBadgeNight")) {
					// Night badge earned
					Statistics.AddBadge("imgBadgeNight");
					CustomMessageBoxMethods.ShowMessage("imgBadgeNight", winBadgeBox);
				}
				App.Current.Properties["exodusBadge"] = "";
				App.Current.Properties["ruthBadge"] = "";
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenMatchHebrew_Click(object sender, RoutedEventArgs e) {
			try {
				HebrewMatch pHebrewMatch = new HebrewMatch();
				NavigationService.Navigate(pHebrewMatch);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenReorderHebrew_Click(object sender, RoutedEventArgs e) {
			try {
				HebrewReorder pHebrewReorder = new HebrewReorder();
				NavigationService.Navigate(pHebrewReorder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenMatchGreek_Click(object sender, RoutedEventArgs e) {
			try {
				GreekMatch pGreekMatch = new GreekMatch();
				NavigationService.Navigate(pGreekMatch);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenReorderGreek_Click(object sender, RoutedEventArgs e) {
			try {
				GreekReorder pGreekReorder = new GreekReorder();
				NavigationService.Navigate(pGreekReorder);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenStatistics_Click(object sender, RoutedEventArgs e) {
			try {
				StatisticsPage pStatistics = new StatisticsPage();
				NavigationService.Navigate(pStatistics);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenSettings_Click(object sender, RoutedEventArgs e) {
			try {
				Settings pSettings = new Settings();
				NavigationService.Navigate(pSettings);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void BtnChangeUser_Click(object sender, RoutedEventArgs e) {
			try {
				SelectUser pSelectUser = new SelectUser();
				NavigationService.Navigate(pSelectUser);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
