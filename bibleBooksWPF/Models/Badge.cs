using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;
using System;
using System.Windows;
using System.Collections.Generic;
using System.Linq;

using BibleBooksWPF.Views;

namespace BibleBooksWPF.Models {
	public class Badge {
		public string strImgName { get; set; }
		public bool blnObtained { get; set; }
		public string strToolTip {
			get {
				return dctToolTips.ContainsKey(strImgName) ? dctToolTips[strImgName] : null;
			}
			set {
				strToolTip = value;
			}
		}

		public static Dictionary<string, string> dctToolTips = new Dictionary<string, string> {
			{ "imgFirstHebrewMatch", "FirstHebrewMatch" },
			{ "imgFirstHebrewReorder", "FirstHebrewReorder" },
			{ "imgFirstGreekMatch", "FirstGreekMatch" },
			{ "imgFirstGreekReorder", "FirstGreekReorder" },
			{ "imgHebrewMatchTime", "HebrewMatchTime" },
			{ "imgHebrewReorderTime", "HebrewReorderTime" },
			{ "imgGreekMatchTime", "GreekMatchTime" },
			{ "imgGreekReorderTime", "GreekReorderTime" },
			{ "imgHebrewMatch100", "HebrewMatch100" },
			{ "imgHebrewReorder100", "HebrewReorder100" },
			{ "imgGreekMatch100", "GreekMatch100" },
			{ "imgGreekReorder100", "GreekReorder100" },
			{ "imgBadgeMorning", "Morning" },
			{ "imgBadgeNight", "Night" },
			{ "imgBadgeExodus", "BookBadge" },
			{ "imgBadgeRuth", "BookBadge" }
		};

		public static List<string> lstAnimalBadges = new List<string> {
			"imgBadgeBear",
			"imgBadgeButterfly",
			"imgBadgeCat",
			"imgBadgeCow",
			"imgBadgeDog",
			"imgBadgeDolphin",
			"imgBadgeElephant",
			"imgBadgeFlamingo",
			"imgBadgeFox",
			"imgBadgeGiraffe",
			"imgBadgeHorse",
			"imgBadgeKoala",
			"imgBadgeLion",
			"imgBadgePanda",
			"imgBadgePenguin",
			"imgBadgeSnake",
			"imgBadgeTiger",
			"imgBadgeTurtle",
			"imgBadgeZebra"
		};

		public Badge(string strImgName, bool blnObtained) {
			this.strImgName = strImgName;
			this.blnObtained = blnObtained;
		}

		public static void AddBadgeForCurrentUser(string strBadgeName) {
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

		public static List<string> GetObtainedBadgeNames() {
			// Get the current user's badges
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
			List<string> lstBadgeNames = userBadgesToken.ToObject<List<string>>();

			return lstBadgeNames;
		}

		public static List<Badge> GetBadgeObjectList() {
			List<Badge> lstBadges = new List<Badge>();

			List<string> lstBadgeNames = GetObtainedBadgeNames();

			foreach (string strImgName in lstBadgeNames) {
				lstBadges.Add(new Badge(strImgName, true));
			}

			return lstBadges;
		}

		public static List<Badge> GetBadgesRequiredToShow() {
			List<Badge> lstBadges = new List<Badge>();
			List<Badge> lstObtainedBadges = GetBadgeObjectList();

			foreach (var pair in dctToolTips) {
				new Badge(pair.Key, lstObtainedBadges.Any(b => b.strImgName == pair.Key));
			}

			return lstBadges;
		}
	}
}
