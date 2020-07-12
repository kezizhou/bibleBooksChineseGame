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
			// Get the current user's statistics for this game
			JObject obj = JObject.Parse(File.ReadAllText("users.json"));
			JToken userGameToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')].lstGameStatistics.GameStatistics[?(@.strName == '" + strGameName + "')]");

			GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();
			gsTotalGames.lintPoints.Add(intPoints);
			gsTotalGames.ltsTimeElapsed.Add(tsTimeElapsed);

			// Update the json with new statistic
			userGameToken.Replace(JToken.FromObject(gsTotalGames));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText("users.json", newJson);
		}
	}

	public static class CustomMessageBoxMethods {
		public static string ShowMessage(string strMessage, string strTitle, string strIcon, BibleBooksWPF.CustomMessageBox winMsgBox) {
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

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, BibleBooksWPF.CustomInputBox winInputBox) {
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
	}
}
