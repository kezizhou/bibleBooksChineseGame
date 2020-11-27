using BibleBooksWPF.Models;
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
				// Get the current user's statistics for this game
				JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
				JToken userGameToken = obj.SelectToken($"$.Users[?(@.username == '{App.Current.Properties["currentUsername"]}')].lstGameStatistics.GameStatistics[?(@.strName == '{strGameName}')]");

				GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();

				// Get the current user's existing badges
				JToken userBadgesToken = obj.SelectToken($"$.Users[?(@.username == '{App.Current.Properties["currentUsername"]}')].lstBadges");
				List<string> lstBadges = userBadgesToken.ToObject<List<string>>();

				// Check if this is the first game
				if (gsTotalGames.lintPoints.Count == 0) {
					// Add new badge
					Badge.AddBadgeForCurrentUser("imgFirst" + strGameName);
					NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgFirst" + strGameName);

					// Refresh json object
					obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
					userGameToken = obj.SelectToken($"$.Users[?(@.username == '{App.Current.Properties["currentUsername"]}')].lstGameStatistics.GameStatistics[?(@.strName == '{strGameName}')]");
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
					Badge.AddBadgeForCurrentUser("imgBadgeExodus");
					NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgBadgeExodus");
					App.Current.Properties["exodusBadge"] = "added";
				}

				// Ruth badge
				if (App.Current.Properties["ruthBadge"].ToString() == "both" && !lstBadges.Contains("imgBadgeRuth")) {
					Badge.AddBadgeForCurrentUser("imgBadgeRuth");
					NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgBadgeRuth");
					App.Current.Properties["ruthBadge"] = "added";
				}

				if (strGameName.Equals("HebrewMatch")) {
					if (CheckHebrewMatchBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(ref obj, "HebrewMatch");
					}
				} else if (strGameName.Equals("HebrewReorder")) {
					if (CheckHebrewReorderBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(ref obj, "HebrewReorder");
					}
				} else if (strGameName.Equals("GreekMatch")) {
					if (CheckGreekMatchBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(ref obj, "GreekMatch");
					}
				} else if (strGameName.Equals("GreekReorder")) {
					if (CheckGreekReorderBadges(lstBadges, tsTimeElapsed, intPoints)) {
						// Refresh json object
						userGameToken = RefreshJObject(ref obj, "GreekReorder");
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

			if (tsTimeElapsed <= new TimeSpan(0, 1, 15) && !lstBadges.Contains("imgHebrewMatchTime")) {
				Badge.AddBadgeForCurrentUser("imgHebrewMatchTime");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgHebrewMatchTime");
			}
			if (intPoints == 39 && !lstBadges.Contains("imgHebrewMatch100")) {
				Badge.AddBadgeForCurrentUser("imgHebrewMatch100");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgHebrewMatch100");
			}

			return blnBadgeAdded;
		}

		public static bool CheckHebrewReorderBadges(List<string> lstBadges, TimeSpan tsTimeElapsed, int intPoints) {
			bool blnBadgeAdded = false;

			if (tsTimeElapsed <= new TimeSpan(0, 2, 15) && !lstBadges.Contains("imgHebrewReorderTime")) {
				Badge.AddBadgeForCurrentUser("imgHebrewReorderTime");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgHebrewReorderTime");
			}
			if (intPoints == 39 && !lstBadges.Contains("imgHebrewReorder100")) {
				Badge.AddBadgeForCurrentUser("imgHebrewReorder100");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgHebrewReorder100");
			}

			return blnBadgeAdded;
		}

		public static bool CheckGreekMatchBadges(List<string> lstBadges, TimeSpan tsTimeElapsed, int intPoints) {
			bool blnBadgeAdded = false;

			if (tsTimeElapsed <= new TimeSpan(0, 0, 45) && !lstBadges.Contains("imgGreekMatchTime")) {
				Badge.AddBadgeForCurrentUser("imgGreekMatchTime");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgGreekMatchTime");
				blnBadgeAdded = true;
			}
			if (intPoints == 27 && !lstBadges.Contains("imgGreekMatch100")) {
				Badge.AddBadgeForCurrentUser("imgGreekMatch100");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgGreekMatch100");
				blnBadgeAdded = true;
			}

			return blnBadgeAdded;
		}

		public static bool CheckGreekReorderBadges(List<string> lstBadges, TimeSpan tsTimeElapsed, int intPoints) {
			bool blnBadgeAdded = false;

			if (tsTimeElapsed <= new TimeSpan(0, 0, 50) && !lstBadges.Contains("imgGreekReorderTime")) {
				Badge.AddBadgeForCurrentUser("imgGreekReorderTime");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgGreekReorderTime");
				blnBadgeAdded = true;
			}
			if (intPoints == 27 && !lstBadges.Contains("imgGreekReorder100")) {
				Badge.AddBadgeForCurrentUser("imgGreekReorder100");
				NewBadgeMessage winBadgeBox = new NewBadgeMessage("imgGreekReorder100");
				blnBadgeAdded = true;
			}

			return blnBadgeAdded;
		}

		public static JToken RefreshJObject(ref JObject obj, string strGameName) {
			obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));

			return obj.SelectToken($"$.Users[?(@.username == '{App.Current.Properties["currentUsername"]}')].lstGameStatistics.GameStatistics[?(@.strName == '{strGameName}')]");
		}
	}
}
