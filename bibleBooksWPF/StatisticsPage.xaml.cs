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

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for Statistics.xaml
	/// </summary>
	public partial class StatisticsPage : Page {
        public StatisticsPage() {
            InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			TimeSpan average = new TimeSpan();
			TimeSpan averageTrim = new TimeSpan();

			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));

			// Load statistic numbers
			// Hebrew Scriptures Matching
			JToken userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'HebrewMatch')]");
			GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblHebrewRecordPtM.Content += gsTotalGames.lintPoints.Max().ToString();
				lblHebrewRecordTimeM.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblHebrewAveragePtM.Content += gsTotalGames.lintPoints.Average().ToString();;
				average = TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds));
				averageTrim = new TimeSpan(average.Hours, average.Minutes, average.Seconds);
				lblHebrewAverageTimeM.Content += averageTrim.ToString();
			}

			// Hebrew Scriptures Reorder
			userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'HebrewReorder')]");
			gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblHebrewRecordPtR.Content += gsTotalGames.lintPoints.Max().ToString();
				lblHebrewRecordTimeR.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblHebrewAveragePtR.Content += gsTotalGames.lintPoints.Average().ToString();
				average = TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds));
				averageTrim = new TimeSpan(average.Hours, average.Minutes, average.Seconds);
				lblHebrewAverageTimeR.Content += averageTrim.ToString();
			}

			// Greek Scriptures Matching
			userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'GreekMatch')]");
			gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblGreekRecordPtM.Content += gsTotalGames.lintPoints.Max().ToString();
				lblGreekRecordTimeM.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblGreekAveragePtM.Content += gsTotalGames.lintPoints.Average().ToString();
				average = TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds));
				averageTrim = new TimeSpan(average.Hours, average.Minutes, average.Seconds);
				lblGreekAverageTimeM.Content += averageTrim.ToString();
			}

			// Greek Scriptures Reorder
			userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName ==  'GreekReorder')]");
			gsTotalGames = userGameToken.ToObject<GameStatistics>();
			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				lblGreekRecordPtR.Content += gsTotalGames.lintPoints.Max().ToString();
				lblGreekRecordTimeR.Content += gsTotalGames.ltsTimeElapsed.Min().ToString();
				lblGreekAveragePtR.Content += gsTotalGames.lintPoints.Average().ToString();
				average = TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds));
				averageTrim = new TimeSpan(average.Hours, average.Minutes, average.Seconds);
				lblGreekAverageTimeR.Content += averageTrim.ToString();
			}

			// Load total points
			txbTotalPoints.Text = Properties.Settings.Default.lngTotalPoints.ToString();

			// Load badges
			JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
			List<string> lstBadges = userBadgesToken.ToObject<List<string>>();
			foreach (string strBadgeName in lstBadges) {
				Image img = (Image)this.FindName(strBadgeName);
				img.Opacity = 1;
				img.Visibility = Visibility.Visible;
			}
		}

		private void BtnSubmitCode_Click(object sender, RoutedEventArgs e) {
			if (txtCode.Text.Length == 0 || txtCode.Text.Length == 1) {
				// Empty string or 1 character
				txbInvalid.Visibility = Visibility.Visible;
				asyncRemoveInvalid();
			} else if (this.FindName("imgBadge" + char.ToUpper(txtCode.Text.ToLower()[0]) + txtCode.Text.Substring(1)) != null) {
				Image imgBadge = this.FindName("imgBadge" + char.ToUpper(txtCode.Text.ToLower()[0]) + txtCode.Text.Substring(1)) as Image;
				imgBadge.Visibility = Visibility.Visible;
				Statistics.AddBadge(imgBadge.Name);
			} else {
				// Invalid code
				txbInvalid.Visibility = Visibility.Visible;
				asyncRemoveInvalid();
			}

			txtCode.Clear();
		}

		private async void asyncRemoveInvalid() {
			await Task.Delay(1200);
			txbInvalid.Visibility = Visibility.Hidden;
		}
	}
}
