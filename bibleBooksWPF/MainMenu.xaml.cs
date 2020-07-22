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
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
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
		}

		private void ImenMatchHebrew_Click(object sender, RoutedEventArgs e) {
			HebrewMatch pHebrewMatch = new HebrewMatch();
			NavigationService.Navigate(pHebrewMatch);
		}

		private void ImenReorderHebrew_Click(object sender, RoutedEventArgs e) {
			HebrewReorder pHebrewReorder = new HebrewReorder();
			NavigationService.Navigate(pHebrewReorder);
		}

		private void ImenMatchGreek_Click(object sender, RoutedEventArgs e) {
			GreekMatch pGreekMatch = new GreekMatch();
			NavigationService.Navigate(pGreekMatch);
		}

		private void ImenReorderGreek_Click(object sender, RoutedEventArgs e) {
			GreekReorder pGreekReorder = new GreekReorder();
			NavigationService.Navigate(pGreekReorder);
		}

		private void ImenStatistics_Click(object sender, RoutedEventArgs e) {
			StatisticsPage pStatistics = new StatisticsPage();
			NavigationService.Navigate(pStatistics);
		}

		private void ImenSettings_Click(object sender, RoutedEventArgs e) {
			Settings pSettings = new Settings();
			NavigationService.Navigate(pSettings);
		}

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void BtnChangeUser_Click(object sender, RoutedEventArgs e) {
			SelectUser pSelectUser = new SelectUser();
			NavigationService.Navigate(pSelectUser);
		}
	}
}
