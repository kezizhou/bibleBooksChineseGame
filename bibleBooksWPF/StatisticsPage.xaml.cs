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
			// Language setting
			if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
				// Menu bar
				imenMainMenu.Header = "主菜单";
				imenHebrew.Header = "希伯来语经卷";
				imenMatchHebrew.Header = "中英文配对";
				imenReorderHebrew.Header = "排序";
				imenGreek.Header = "希腊语经卷";
				imenMatchGreek.Header = "中英文配对";
				imenReorderGreek.Header = "排序";
				imenStatistics.Header = "成绩";
				imenSettings.Header = "设置";
				imenExit.Header = "退出";
				menTop.FontSize = 16;

				// Hebrew Scriptures
				grpHebrew.Header = "希伯来语经卷";
				// Matching
				txbMatchingH.Text = "配对";
				lblHebrewRecordPtMDesc.Content = "最高分数";
				lblHebrewRecordTimeMDesc.Content = "最快时间";
				lblHebrewAveragePtMDesc.Content = "平均分数";
				lblHebrewAverageTimeMDesc.Content = "平均时间";
				// Reorder
				txbReorderH.Text = "排序";
				lblHebrewRecordPtRDesc.Content = "最高分数";
				lblHebrewRecordTimeRDesc.Content = "最快时间";
				lblHebrewAveragePtRDesc.Content = "平均分数";
				lblHebrewAverageTimeRDesc.Content = "平均时间";

				// Greek Scriptures
				grpGreek.Header = "希腊语经卷";
				// Matching
				txbMatchingG.Text = "配对";
				lblGreekRecordPtMDesc.Content = "最高分数";
				lblGreekRecordTimeMDesc.Content = "最快时间";
				lblGreekAveragePtMDesc.Content = "平均分数";
				lblGreekAverageTimeMDesc.Content = "平均时间";
				// Reorder
				txbReorderG.Text = "排序";
				lblGreekRecordPtRDesc.Content = "最高分数";
				lblGreekRecordTimeRDesc.Content = "最快时间";
				lblGreekAveragePtRDesc.Content = "平均分数";
				lblGreekAverageTimeRDesc.Content = "平均时间";

				// Achievements
				grpAchievements.Header = "成绩";
				txbBadges.Text = "奖章";
				// Badge Tooltips
				imgFirstHebrewMatch.ToolTip = "玩一次希伯来语经卷配对";
				imgFirstHebrewReorder.ToolTip = "玩一次希伯来语经卷排序";
				imgFirstGreekMatch.ToolTip = "玩一次希腊语经卷配对";
				imgFirstGreekReorder.ToolTip = "玩一次希腊语经卷排序";
				imgHebrewMatchTime.ToolTip = "希伯来语经卷配对时间少于00:01:15";
				imgHebrewReorderTime.ToolTip = "希伯来语经卷排序时间少于00:02:15";
				imgGreekMatchTime.ToolTip = "希腊语经卷配对时间少于00:00:45";
				imgGreekReorderTime.ToolTip = "希腊语经卷排序时间少于00:00:50";
				imgHebrewMatch100.ToolTip = "希伯来语经卷配对100%准确率";
				imgHebrewReorder100.ToolTip = "希伯来语经卷排序100%准确率";
				imgGreekMatch100.ToolTip = "希腊语经卷配对100%准确率";
				imgGreekReorder100.ToolTip = "希腊语经卷排序100%准确率";
				imgBadgeMorning.ToolTip = "8:30am 之前玩";
				imgBadgeNight.ToolTip = "9:00pm 以后玩";
				imgBadgeExodus.ToolTip = "把这本书在两个不同的游戏中答对";
				imgBadgeRuth.ToolTip = "把这本书在两个不同的游戏中答对";

				// Enter a Code
				txbEnterCode.Text = "输入密码";
				btnSubmitCode.Content = "递交";
				txbInvalid.Text = "密码无效。";
			}

			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));

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
