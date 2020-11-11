using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BibleBooksWPF.Helpers {
	public static class StatisticMethods {
		public static string AddStatistic(string strGameName, int intPoints, TimeSpan tsTimeElapsed) {
			string strRecord = "";

			try {

				NewBadgeMessage winBadgeBox;

				// Get the current user's statistics for this game
				JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
				JToken userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");

				GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();

				// Check if this is the first game
				if (gsTotalGames.lintPoints.Count == 0) {
					// Add new badge
					AddBadge("imgFirst" + strGameName);
					winBadgeBox = new NewBadgeMessage();
					CustomMessageBoxMethods.ShowMessage("imgFirst" + strGameName, winBadgeBox);

					// Refresh json object
					obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
					userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
				} else {
					// Check if this is a new high score
					if ((intPoints > gsTotalGames.lintPoints.Max()) && (tsTimeElapsed < gsTotalGames.ltsTimeElapsed.Min())) {
						strRecord = "both";
					} else if (tsTimeElapsed < gsTotalGames.ltsTimeElapsed.Min()) {
						strRecord = "time";
					} else if (intPoints > gsTotalGames.lintPoints.Max()) {
						strRecord = "point";
					}
				}

				// Exodus badge
				if (App.Current.Properties["exodusBadge"].ToString() == "both") {
					AddBadge("imgBadgeExodus");
					winBadgeBox = new NewBadgeMessage();
					CustomMessageBoxMethods.ShowMessage("imgBadgeExodus", winBadgeBox);
					App.Current.Properties["exodusBadge"] = "added";
				}

				// Ruth badge
				if (App.Current.Properties["ruthBadge"].ToString() == "both") {
					AddBadge("imgBadgeRuth");
					winBadgeBox = new NewBadgeMessage();
					CustomMessageBoxMethods.ShowMessage("imgBadgeRuth", winBadgeBox);
					App.Current.Properties["ruthBadge"] = "added";
				}

				if (strGameName.Equals("HebrewMatch")) {
					if (tsTimeElapsed <= new TimeSpan(0, 1, 15)) {
						AddBadge("imgHebrewMatchTime");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgHebrewMatchTime", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
					if (intPoints == 39) {
						AddBadge("imgHebrewMatch100");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgHebrewMatch100", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
				} else if (strGameName.Equals("HebrewReorder")) {
					if (tsTimeElapsed <= new TimeSpan(0, 2, 15)) {
						AddBadge("imgHebrewReorderTime");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgHebrewReorderTime", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
					if (intPoints == 39) {
						AddBadge("imgHebrewReorder100");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgHebrewReorder100", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
				} else if (strGameName.Equals("GreekMatch")) {
					if (tsTimeElapsed <= new TimeSpan(0, 0, 45)) {
						AddBadge("imgGreekMatchTime");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgGreekMatchTime", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
					if (intPoints == 27) {
						AddBadge("imgGreekMatch100");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgGreekMatch100", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
				} else if (strGameName.Equals("GreekReorder")) {
					if (tsTimeElapsed <= new TimeSpan(0, 0, 50)) {
						AddBadge("imgGreekReorderTime");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgGreekReorderTime", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
					if (intPoints == 27) {
						AddBadge("imgGreekReorder100");
						winBadgeBox = new NewBadgeMessage();
						CustomMessageBoxMethods.ShowMessage("imgGreekReorder100", winBadgeBox);

						// Refresh json object
						obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
					}
				}

				gsTotalGames.lintPoints.Add(intPoints);
				gsTotalGames.ltsTimeElapsed.Add(tsTimeElapsed);

				// Update the json with new statistic
				userGameToken.Replace(JToken.FromObject(gsTotalGames));

				string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
				File.WriteAllText(Globals.usersFilePath, newJson);

			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strRecord;
		}

		public static void AddBadge(string strBadgeName) {
			try {
				// Get the current user's badges
				JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
				JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
				List<string> lstBadges = userBadgesToken.ToObject<List<string>>();

				// Add the new badge if not already awarded
				if (!lstBadges.Contains(strBadgeName)) {
					lstBadges.Add(strBadgeName);
				}

				// Update the json with new statistic
				userBadgesToken.Replace(JToken.FromObject(lstBadges));

				string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
				File.WriteAllText(Globals.usersFilePath, newJson);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		public static List<string> GetBadges() {
			// Get the current user's badges
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
			List<string> lstBadges = userBadgesToken.ToObject<List<string>>();

			return lstBadges;
		}
	}
}
