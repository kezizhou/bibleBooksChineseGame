using BibleBooksWPF.Views;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;

namespace BibleBooksWPF.Helpers {
	public class StatisticMethods {
		public static string AddStatistic(string strGameName, int intPoints, TimeSpan tsTimeElapsed) {
			string strRecord = "";

			try {

				NewBadgeMessage winBadgeBox;

				// Get the current user's statistics for this game
				JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
				JToken userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");

				GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();

				// Get the current user's existing badges
				JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
				List<string> lstBadges = userBadgesToken.ToObject<List<string>>();

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
				if (App.Current.Properties["exodusBadge"].ToString() == "both" && !lstBadges.Contains("imgBadgeExodus")) {
					AddBadge("imgBadgeExodus");
					winBadgeBox = new NewBadgeMessage();
					CustomMessageBoxMethods.ShowMessage("imgBadgeExodus", winBadgeBox);
					App.Current.Properties["exodusBadge"] = "added";
				}

				// Ruth badge
				if (App.Current.Properties["ruthBadge"].ToString() == "both" && !lstBadges.Contains("imgBadgeRuth")) {
					AddBadge("imgBadgeRuth");
					winBadgeBox = new NewBadgeMessage();
					CustomMessageBoxMethods.ShowMessage("imgBadgeRuth", winBadgeBox);
					App.Current.Properties["ruthBadge"] = "added";
				}

				if (strGameName.Equals("HebrewMatch")) {
					if (CheckHebrewMatchBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(obj, "HebrewMatch");
					}
				} else if (strGameName.Equals("HebrewReorder")) {
					if (CheckHebrewReorderBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(obj, "HebrewReorder");
					}
				} else if (strGameName.Equals("GreekMatch")) {
					if (CheckGreekMatchBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(obj, "GreekMatch");
					}
				} else if (strGameName.Equals("GreekReorder")) {
					if (CheckGreekReorderBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(obj, "GreekReorder");
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

		public static bool CheckHebrewMatchBadges(List<string> lstBadges, TimeSpan tsTimeElapsed, int intPoints) {
			bool blnBadgeAdded = false;
			NewBadgeMessage winBadgeBox;

			if (tsTimeElapsed <= new TimeSpan(0, 1, 15) && !lstBadges.Contains("imgHebrewMatchTime")) {
				AddBadge("imgHebrewMatchTime");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgHebrewMatchTime", winBadgeBox);
			}
			if (intPoints == 39 && !lstBadges.Contains("imgHebrewMatch100")) {
				AddBadge("imgHebrewMatch100");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgHebrewMatch100", winBadgeBox);
			}

			return blnBadgeAdded;
		}

		public static bool CheckHebrewReorderBadges(List<string> lstBadges, TimeSpan tsTimeElapsed, int intPoints) {
			bool blnBadgeAdded = false;
			NewBadgeMessage winBadgeBox;

			if (tsTimeElapsed <= new TimeSpan(0, 2, 15) && !lstBadges.Contains("imgHebrewReorderTime")) {
				AddBadge("imgHebrewReorderTime");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgHebrewReorderTime", winBadgeBox);
			}
			if (intPoints == 39 && !lstBadges.Contains("imgHebrewReorder100")) {
				AddBadge("imgHebrewReorder100");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgHebrewReorder100", winBadgeBox);
			}

			return blnBadgeAdded;
		}

		public static bool CheckGreekMatchBadges(List<string> lstBadges, TimeSpan tsTimeElapsed, int intPoints) {
			bool blnBadgeAdded = false;
			NewBadgeMessage winBadgeBox;

			if (tsTimeElapsed <= new TimeSpan(0, 0, 45) && !lstBadges.Contains("imgGreekMatchTime")) {
				AddBadge("imgGreekMatchTime");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgGreekMatchTime", winBadgeBox);
				blnBadgeAdded = true;
			}
			if (intPoints == 27 && !lstBadges.Contains("imgGreekMatch100")) {
				AddBadge("imgGreekMatch100");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgGreekMatch100", winBadgeBox);
				blnBadgeAdded = true;
			}

			return blnBadgeAdded;
		}

		public static bool CheckGreekReorderBadges(List<string> lstBadges, TimeSpan tsTimeElapsed, int intPoints) {
			bool blnBadgeAdded = false;
			NewBadgeMessage winBadgeBox;

			if (tsTimeElapsed <= new TimeSpan(0, 0, 50) && !lstBadges.Contains("imgGreekReorderTime")) {
				AddBadge("imgGreekReorderTime");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgGreekReorderTime", winBadgeBox);
				blnBadgeAdded = true;
			}
			if (intPoints == 27 && !lstBadges.Contains("imgGreekReorder100")) {
				AddBadge("imgGreekReorder100");
				winBadgeBox = new NewBadgeMessage();
				CustomMessageBoxMethods.ShowMessage("imgGreekReorder100", winBadgeBox);
				blnBadgeAdded = true;
			}

			return blnBadgeAdded;
		}

		public static JToken RefreshJObject(JObject obj, string strGameName) {
			obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));

			return obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
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
