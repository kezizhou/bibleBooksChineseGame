using ExtensionMethods;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for SelectUser.xaml
	/// </summary>
	public partial class SelectUser : Page {

		RootUser lstUsers = new RootUser();
		User currentUser = new User();

		public SelectUser() {
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			// Create file if it does not exist
			if (!File.Exists("users.json")) {
				File.Create("users.json");
			}

			// Get the users from the file if not empty
			if (new FileInfo("users.json").Length != 0) {
				using (StreamReader file = File.OpenText("users.json")) {
					JsonSerializer serializer = new JsonSerializer();
					//lstUsers = JObject.Parse(File.ReadAllText("users.json")).SelectToken("Users").ToObject<RootUser>();
					lstUsers = serializer.Deserialize(file, typeof(RootUser)) as RootUser;
				}

				// Loop through users and fill the buttons in with the username
				int i = 1;
				foreach (User user in lstUsers.Users) {
					Button btn = this.FindName("btnUser" + i) as Button;
					btn.Content = user.username;
					i += 1;
				}
			}
		}

		private void BtnUser1_Click(object sender, RoutedEventArgs e) {
			Button btnUser1 = sender as Button;
			if (btnUser1.Content.Equals("New User")) {
				NewUser();
			} else {
				// Existing user
				LoadExistingUser((string)btnUser1.Content);
			}
			MainMenu pMainMenu = new MainMenu();
			NavigationService.Navigate(pMainMenu);
		}

		private void BtnUser2_Click(object sender, RoutedEventArgs e) {
			Button btnUser2 = sender as Button;
			if (btnUser2.Content.Equals("New User")) {
				NewUser();
			} else {
				// Existing user
				LoadExistingUser((string)btnUser2.Content);
			}
			MainMenu pMainMenu = new MainMenu();
			NavigationService.Navigate(pMainMenu);
		}

		private void BtnUser3_Click(object sender, RoutedEventArgs e) {
			Button btnUser3 = sender as Button;
			if (btnUser3.Content.Equals("New User")) {
				NewUser();
			} else {
				// Existing user
				LoadExistingUser((string) btnUser3.Content);
			}
			MainMenu pMainMenu = new MainMenu();
			NavigationService.Navigate(pMainMenu);
		}

		private void NewUser() {
			CustomInputBox winInputBox = new CustomInputBox();

			string strResponse = CustomMessageBoxMethods.ShowMessage("Please enter your username:", "New User", "newUser", winInputBox);

			switch (strResponse) {
				case "Cancel":
					break;
				default:
					CreateUserData(strResponse);
					MessageBox.Show("New user " + strResponse + " created.");
					break;
			}
		}

		private void CreateUserData(string strUsername) {
			// Create new statistics and user
			GameStatistics gsHebrewMatch = new GameStatistics("HebrewMatch");
			GameStatistics gsHebrewReorder = new GameStatistics("HebrewReorder");
			GameStatistics gsGreekMatch = new GameStatistics("GreekMatch");
			GameStatistics gsGreekReorder = new GameStatistics("GreekReorder");

			RootGameStatistics lstStatistics = new RootGameStatistics(new[] {
				gsHebrewMatch, gsHebrewReorder, gsGreekMatch, gsGreekReorder
			});
			User currentUser = new User(strUsername, lstStatistics);

			App.Current.Properties["currentUsername"] = currentUser.username;

			lstUsers.Users.Add(currentUser);

			// Add to JSON file
			using (StreamWriter file = File.CreateText("users.json")) {
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, lstUsers);
			}

		}

		private void LoadExistingUser(string strUsername) {
			App.Current.Properties["currentUsername"] = strUsername;

		}
	}
}
