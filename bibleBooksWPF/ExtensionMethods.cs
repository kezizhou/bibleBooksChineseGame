using BibleBooksWPF;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ExtensionMethods {
    public static class LabelExt {
		public static void BringToFront(this Label lbl) {
			try {
				if (lbl == null) return;

				Grid parent = lbl.Parent as Grid;
				if (parent == null) return;

				var maxZ = parent.Children.OfType<UIElement>()
				  .Where(x => x != lbl)
				  .Select(x => Grid.GetZIndex(x))
				  .Max();
				Grid.SetZIndex(lbl, maxZ + 1);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}

	public static class Statistics {
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
					if ( (intPoints > gsTotalGames.lintPoints.Max()) && (tsTimeElapsed < gsTotalGames.ltsTimeElapsed.Min()) ) {
						strRecord = "both";
					} else if (tsTimeElapsed < gsTotalGames.ltsTimeElapsed.Min()) {
						strRecord = "time";
					} else if (intPoints > gsTotalGames.lintPoints.Max()) {
						strRecord = "point";
					}
				}

				// Exodus badge
				if (App.Current.Properties["exodusBadge"].ToString() == "2") {
					AddBadge("imgBadgeExodus");
					winBadgeBox = new NewBadgeMessage();
					CustomMessageBoxMethods.ShowMessage("imgBadgeExodus", winBadgeBox);
				}

				// Ruth badge
				if (App.Current.Properties["ruthBadge"].ToString() == "2") {
					AddBadge("imgBadgeRuth");
					winBadgeBox = new NewBadgeMessage();
					CustomMessageBoxMethods.ShowMessage("imgBadgeRuth", winBadgeBox);
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
					if (intPoints == 39 ) {
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

	public static class CustomMessageBoxMethods {
		public static string ShowMessage(string strMessage, string strTitle, string strIcon, CustomMessageBox winMsgBox) {
			string strResult = "";
			
			try {
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
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, string strRecord, CustomMessageBox winMsgBox) {
			string strResult = "";

			try {
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

				// Show record message
				Image imgTime = winMsgBox.FindName("imgTimeRecord") as Image;
				Image imgPoint = winMsgBox.FindName("imgPointRecord") as Image;
				TextBlock txbRecord = winMsgBox.FindName("txbRecord") as TextBlock;
				switch (strRecord) {
					case "both":
						imgTime.Visibility = Visibility.Visible;
						imgPoint.Visibility = Visibility.Visible;
						txbRecord.Text = "New records!";
						txbRecord.Visibility = Visibility.Visible;
						break;
					case "time":
						imgTime.Visibility = Visibility.Visible;
						txbRecord.Visibility = Visibility.Visible;
						break;
					case "point":
						imgPoint.Visibility = Visibility.Visible;
						txbRecord.Visibility = Visibility.Visible;
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
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, CustomInputBox winInputBox) {
			string strResult = "";

			try {
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
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static void ShowMessage(string strBadgeName, NewBadgeMessage winBadgeBox) {
			try {
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
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, string strValidation, CustomInputBox winInputBox) {
			string strResult = "";

			try {
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
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static (string, string) ShowMessage(NewUser winNewUser, string strValidation) {
			string strResult = "";
			string strPicture = "";

			try {
				// Set validation error message
				TextBlock txbValidation = winNewUser.FindName("txbValidation") as TextBlock;
				txbValidation.Text = strValidation;
				txbValidation.Visibility = Visibility.Visible;

				// Input text
				TextBox txt = winNewUser.FindName("txtInputText") as TextBox;

				// Profile picture wrap panels
				WrapPanel wrpMales = winNewUser.FindName("wrpMales") as WrapPanel;
				WrapPanel wrpFemales = winNewUser.FindName("wrpFemales") as WrapPanel;

				// Pop-up message box
				winNewUser.ShowDialog();

				// Get return value
				if (winNewUser.strMsgReturn == null) {
					strResult = "Cancel";
				} else if (winNewUser.strMsgReturn.Equals("")) {
					// Username was entered
					// Set username and profile picture
					strResult = txt.Text;

					// Find which profile picture was selected
					if (wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					} else if (wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					}
				} else {
					strResult = winNewUser.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return (strResult, strPicture);
		}

		public static (string,string) ShowMessage(NewUser winNewUser) {
			string strResult = "";
			string strPicture = "";

			try {
				// Input text
				TextBox txt = winNewUser.FindName("txtInputText") as TextBox;

				// Profile picture wrap panels
				WrapPanel wrpMales = winNewUser.FindName("wrpMales") as WrapPanel;
				WrapPanel wrpFemales = winNewUser.FindName("wrpFemales") as WrapPanel;

				// Pop-up message box
				winNewUser.ShowDialog();

				// Get return value
				if (winNewUser.strMsgReturn == null) {
					strResult = "Cancel";
				} else if (winNewUser.strMsgReturn.Equals("")) {
					// Username was entered
					// Set username and profile picture
					strResult = txt.Text;

					// Find which profile picture was selected
					if (wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					} else if (wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					}
				} else {
					strResult = winNewUser.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return (strResult,strPicture);
		}

		public static string ShowMessage(PauseMenu winPause) {
			string strResult = "";

			// Pop-up message box and get return value
			winPause.ShowDialog();
			if (winPause.strMsgReturn == null) {
				strResult = "Resume";
			} else {
				strResult = winPause.strMsgReturn;
			}

			return strResult;
		}

		public static string ShowMessage(string strText, ConfirmMessageBox winConfirm) {
			string strResult = "";

			// Message text 
			TextBlock txb = winConfirm.FindName("txbMessageText") as TextBlock;
			txb.Text = strText;

			// Pop-up message box and get return value
			winConfirm.ShowDialog();
			if (winConfirm.strMsgReturn == null) {
				strResult = "Cancel";
			} else {
				strResult = winConfirm.strMsgReturn;
			}

			return strResult;
		}
	}
}
