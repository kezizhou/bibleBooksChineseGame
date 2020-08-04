using System;
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
		public static int intTotalPoints = 0;

		public MainMenu() {
			try {
				InitializeComponent();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				lblWelcome.Content = "Welcome: " + App.Current.Properties["currentUsername"];
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
				App.Current.Properties["exodusBadge"] = "0";
				App.Current.Properties["ruthBadge"] = "0";
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
