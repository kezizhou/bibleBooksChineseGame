using BibleBooksWPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ExtensionMethods {
    public static class LabelExt {
		public static void BringToFront(this Label lbl) {
			if (lbl == null) return;

			Grid parent = lbl.Parent as Grid;
			if (parent == null) return;

			var maxZ = parent.Children.OfType<UIElement>()
			  .Where(x => x != lbl)
			  .Select(x => Grid.GetZIndex(x))
			  .Max();
			Grid.SetZIndex(lbl, maxZ + 1);
		}
	}

	public static class Statistics {
		public static void AddStatistic(string strGameName, int intPoints, TimeSpan tsTimeElapsed) {
			NewBadgeMessage winBadgeBox = new NewBadgeMessage();

			// Get the current user's statistics for this game
			JObject obj = JObject.Parse(File.ReadAllText("users.json"));
			JToken userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");

			GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();

			// Check if this is the first game
			if (gsTotalGames.lintPoints.Count == 0) {
				// Add new badge
				AddBadge("imgFirst" + strGameName);
				CustomMessageBoxMethods.ShowMessage("imgFirst" + strGameName, winBadgeBox);

				// Refresh json object
				obj = JObject.Parse(File.ReadAllText("users.json"));
				userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");
			}

			if (App.Current.Properties["exodusBadge"].ToString() == "2") {
				AddBadge("imgBadgeExodus");
				CustomMessageBoxMethods.ShowMessage("imgBadgeExodus", winBadgeBox);
			}

			if (App.Current.Properties["ruthBadge"].ToString() == "2") {
				AddBadge("imgBadgeRuth");
				CustomMessageBoxMethods.ShowMessage("imgBadgeRuth", winBadgeBox);
			}

			gsTotalGames.lintPoints.Add(intPoints);
			gsTotalGames.ltsTimeElapsed.Add(tsTimeElapsed);

			// Update the json with new statistic
			userGameToken.Replace(JToken.FromObject(gsTotalGames));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText("users.json", newJson);
		}

		public static void AddBadge(string strBadgeName) {
			// Get the current user's badges
			JObject obj = JObject.Parse(File.ReadAllText("users.json"));
			JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
			List<string> lstBadges = userBadgesToken.ToObject<List<string>>();

			// Add the new badge if not already awarded
			if (!lstBadges.Contains(strBadgeName)) {
				lstBadges.Add(strBadgeName);
			}
			
			// Update the json with new statistic
			userBadgesToken.Replace(JToken.FromObject(lstBadges));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText("users.json", newJson);
		}

		public static List<string> GetBadges() {
			// Get the current user's badges
			JObject obj = JObject.Parse(File.ReadAllText("users.json"));
			JToken userBadgesToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstBadges");
			List<string> lstBadges = userBadgesToken.ToObject<List<string>>();

			return lstBadges;
		}
	}

	public static class CustomMessageBoxMethods {
		public static string ShowMessage(string strMessage, string strTitle, string strIcon, CustomMessageBox winMsgBox) {
			string strResult = "";

			// Message text 
			TextBlock txb = winMsgBox.FindName("txbMessageText") as TextBlock;
			txb.Text = strMessage;

			// Title
			winMsgBox.Title = strTitle;

			// Icon
			Image img = winMsgBox.FindName("imgIcon") as Image;

			// Show correct icon according to parameter
			switch (strIcon) {
				case "congrats":
					img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/congrats.png"));
					break;
				default:
					break;
			}

			// Pop-up message box and get return value
			winMsgBox.ShowDialog();
			if (winMsgBox.strMsgReturn == null) {
				strResult = "Main";
			} else {
				strResult = winMsgBox.strMsgReturn;
			}

			return strResult;
		}

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, CustomInputBox winInputBox) {
			string strResult = "";

			// Message text 
			TextBlock txb = winInputBox.FindName("txbMessageText") as TextBlock;
			txb.Text = strMessage;

			// Title
			winInputBox.Title = strTitle;

			// Icon
			Image img = winInputBox.FindName("imgIcon") as Image;

			// Show correct icon according to parameter
			switch (strIcon) {
				case "newUser":
					img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/newuser.png"));
					break;
				default:
					break;
			}

			// Input text
			TextBox txt = winInputBox.FindName("txtInputText") as TextBox;

			// Pop-up message box
			winInputBox.ShowDialog();

			// Get return value
			if (winInputBox.strMsgReturn == null) {
				// Return cancel string
				strResult = "Cancel";
			} else if (winInputBox.strMsgReturn.Equals("")) {
				// Username was entered, return username
				strResult = txt.Text;
			} else {
				// Return cancel string
				strResult = winInputBox.strMsgReturn;
			}

			return strResult;
		}

		public static void ShowMessage(string strBadgeName, NewBadgeMessage winBadgeBox) {
			// Set image to new badge
			Image img = winBadgeBox.FindName("imgBadge") as Image;
			img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/Badges/" + strBadgeName + ".png"));

			// Set description of badge
			Label lblDescription = winBadgeBox.FindName("lblDescription") as Label;
			StatisticsPage pStatistics = new StatisticsPage();
			Image imgBadge = pStatistics.FindName(strBadgeName) as Image;
			lblDescription.Content += imgBadge.ToolTip.ToString();

			// Pop-up message box
			winBadgeBox.ShowDialog();
		}


		public static string ShowMessage(string strMessage, string strTitle, string strIcon, string strValidation, CustomInputBox winInputBox) {
			string strResult = "";

			// Message text 
			TextBlock txb = winInputBox.FindName("txbMessageText") as TextBlock;
			txb.Text = strMessage;

			// Title
			winInputBox.Title = strTitle;

			// Icon
			Image img = winInputBox.FindName("imgIcon") as Image;

			// Show correct icon according to parameter
			switch (strIcon) {
				case "newUser":
					img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/newuser.png"));
					break;
				default:
					break;
			}

			// Set validation error message
			TextBlock txbValidation = winInputBox.FindName("txbValidation") as TextBlock;
			txbValidation.Text = strValidation;
			txbValidation.Visibility = Visibility.Visible;

			// Input text
			TextBox txt = winInputBox.FindName("txtInputText") as TextBox;

			// Pop-up message box
			winInputBox.ShowDialog();

			// Get return value
			if (winInputBox.strMsgReturn == null) {
				// Return cancel string
				strResult = "Cancel";
			}
			else if (winInputBox.strMsgReturn.Equals("")) {
				// Username was entered, return username
				strResult = txt.Text;
			}
			else {
				// Return cancel string
				strResult = winInputBox.strMsgReturn;
			}

			return strResult;
		}
	}
}
