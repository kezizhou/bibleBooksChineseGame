using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using ExtensionMethods;

namespace BibleBooksWPF
{
    /// <summary>
    /// Interaction logic for Statistics.xaml
    /// </summary>
    public partial class StatisticsPage : Page {
        public StatisticsPage() {
            InitializeComponent();
        }

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			JObject obj = JObject.Parse(File.ReadAllText("users.json"));

			// Load statistic numbers
			// Hebrew Scriptures Matching
			JToken userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'HebrewMatch')]");
			GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblHebrewRecordPtM.Content += gsTotalGames.lintPoints.Max().ToString();
				lblHebrewRecordTimeM.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblHebrewAveragePtM.Content += gsTotalGames.lintPoints.Average().ToString();
				lblHebrewAverageTimeM.Content += TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds)).ToString();
			}

			// Hebrew Scriptures Reorder
			userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'HebrewReorder')]");
			gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblHebrewRecordPtR.Content += gsTotalGames.lintPoints.Max().ToString();
				lblHebrewRecordTimeR.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblHebrewAveragePtR.Content += gsTotalGames.lintPoints.Average().ToString();
				lblHebrewAverageTimeR.Content += TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds)).ToString();
			}

			// Greek Scriptures Matching
			userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'GreekMatch')]");
			gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblGreekRecordPtM.Content += gsTotalGames.lintPoints.Max().ToString();
				lblGreekRecordTimeM.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblGreekAveragePtM.Content += gsTotalGames.lintPoints.Average().ToString();
				lblGreekAverageTimeM.Content += TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds)).ToString();
			}

			// Greek Scriptures Reorder
			userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'GreekReorder')]");
			gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblGreekRecordPtR.Content += gsTotalGames.lintPoints.Max().ToString();
				lblGreekRecordTimeR.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblGreekAveragePtR.Content += gsTotalGames.lintPoints.Average().ToString();
				lblGreekAverageTimeR.Content += TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds)).ToString();
			}

			// Load badges
			JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
			List<string> lstBadges = userBadgesToken.ToObject<List<string>>();
			foreach (string strBadgeName in lstBadges) {
				Image img = (Image)this.FindName(strBadgeName);
				img.Opacity = 1;
				if (img.Name.Contains("Special")) {
					img.Visibility = Visibility.Visible;
				}
			}
		}

		private void BtnSubmitCode_Click(object sender, RoutedEventArgs e) {
			// Hidden for Git publish
		}

		private async void asyncRemoveInvalid() {
			await Task.Delay(1200);
			txbInvalid.Visibility = Visibility.Hidden;
		}

		private void ImenMainMenu_Click(object sender, RoutedEventArgs e) {
			MainMenu pMainMenu = new MainMenu();
			NavigationService.Navigate(pMainMenu);
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

		private void ImenSettings_Click(object sender, RoutedEventArgs e) {
			Settings pSettings = new Settings();
			NavigationService.Navigate(pSettings);
		}

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}
	}
}
