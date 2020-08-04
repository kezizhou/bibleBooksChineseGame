using ExtensionMethods;

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for SelectUser.xaml
	/// </summary>
	public partial class SelectUser : Page {

		RootUser lstUsers = new RootUser();
		User currentUser = new User();

		public SelectUser() {
			try {
				InitializeComponent();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				// Create file if it does not exist
				if (!File.Exists("users.json")) {
					File.Create("users.json");
				}

				// Get the users from the file if not empty
				if (new FileInfo("users.json").Length != 0) {
					using (StreamReader file = File.OpenText("users.json")) {
						JsonSerializer serializer = new JsonSerializer();
						lstUsers = serializer.Deserialize(file, typeof(RootUser)) as RootUser;
					}

					// Loop through users
					int i = 1;
					foreach (User user in lstUsers.Users) {
						// Fill button with username
						Button btn = this.FindName("btnUser" + i) as Button;
						btn.Content = user.username;

						// Change profile picture
						Image imgProfile = this.FindName("imgUser" + i) as Image;
						imgProfile.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/ProfileImages/" + user.profilePicture + ".png"));

						// Make delete button visible
						Button btnDelete = this.FindName("btnDelete" + i) as Button;
						btnDelete.Visibility = Visibility.Visible;

						i += 1;
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnUser1_Click(object sender, RoutedEventArgs e) {
			try {
				Button btnUser1 = sender as Button;
				if (btnUser1.Content.Equals("New User")) {
					NewUser();
				} else {
					// Existing user
					LoadExistingUser((string)btnUser1.Content);
					MainMenu pMainMenu = new MainMenu();
					NavigationService.Navigate(pMainMenu);
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnUser2_Click(object sender, RoutedEventArgs e) {
			try {
				Button btnUser2 = sender as Button;
				if (btnUser2.Content.Equals("New User")) {
					NewUser();
				} else {
					// Existing user
					LoadExistingUser((string)btnUser2.Content);
					MainMenu pMainMenu = new MainMenu();
					NavigationService.Navigate(pMainMenu);
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnUser3_Click(object sender, RoutedEventArgs e) {
			try {
				Button btnUser3 = sender as Button;
				if (btnUser3.Content.Equals("New User")) {
					NewUser();
				} else {
					// Existing user
					LoadExistingUser((string)btnUser3.Content);
					MainMenu pMainMenu = new MainMenu();
					NavigationService.Navigate(pMainMenu);
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void NewUser() {
			NewUser winNewUser = new NewUser();

			var newUser = CustomMessageBoxMethods.ShowMessage(winNewUser);
			string[] astrUsernames = new string[3];

			// Create an array of usernames
			for (int i = 0; i < 3; i++) {
				Button btnUser = this.FindName("btnUser" + (i + 1).ToString()) as Button;
				astrUsernames[i] = btnUser.Content.ToString();
			}

			// Check that the input is not an existing user
			while ( ((Array.IndexOf(astrUsernames, newUser.Item1) >= 0) ||  // Is a duplicate username
			newUser.Item1.Equals(string.Empty) ||                           // Is an empty string
			newUser.Item1.Equals("New User") )							// Is "New User"
			&& !newUser.Item1.Equals("Cancel")) {                         // Is not the cancel button

				switch (newUser.Item1) {
					case "":
						winNewUser = new NewUser();
						newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "Please enter a valid username");
						break;
					case "New User":
						winNewUser = new NewUser();
						newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "Please enter a valid username");
						break;
					default:
						winNewUser = new NewUser();
						newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "Please enter a unique username");
						break;
				}

			}

			switch (newUser.Item1) {
				case "Cancel":
					break;
				default:
					CreateUserData(newUser.Item1, newUser.Item2);
					MainMenu pMainMenu = new MainMenu();
					NavigationService.Navigate(pMainMenu);
					break;
			}
		}

		private void CreateUserData(string strUsername, string strProfilePic) {
			// Create new statistics and user
			GameStatistics gsHebrewMatch = new GameStatistics("HebrewMatch");
			GameStatistics gsHebrewReorder = new GameStatistics("HebrewReorder");
			GameStatistics gsGreekMatch = new GameStatistics("GreekMatch");
			GameStatistics gsGreekReorder = new GameStatistics("GreekReorder");
			RootGameStatistics lstStatistics = new RootGameStatistics(new[] {
				gsHebrewMatch, gsHebrewReorder, gsGreekMatch, gsGreekReorder
			});
			List<string> lstBadges = new List<string>();
			User currentUser = new User(strUsername, strProfilePic, lstStatistics, lstBadges);

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

		private void btnDelete1_Click(object sender, RoutedEventArgs e) {
			lstUsers.Users.RemoveAt(0);

			// Update JSON file
			using (StreamWriter file = File.CreateText("users.json")) {
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, lstUsers);
			}

			RefreshPage();
		}

		private void btnDelete2_Click(object sender, RoutedEventArgs e) {
			lstUsers.Users.RemoveAt(1);

			// Update JSON file
			using (StreamWriter file = File.CreateText("users.json")) {
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, lstUsers);
			}

			RefreshPage();
		}

		private void btnDelete3_Click(object sender, RoutedEventArgs e) {
			lstUsers.Users.RemoveAt(2);

			// Update JSON file
			using (StreamWriter file = File.CreateText("users.json")) {
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, lstUsers);
			}

			RefreshPage();
		}

		private void RefreshPage() {
			SelectUser pSelectUser = new SelectUser();
			NavigationService.Navigate(pSelectUser);
		}
	}
}
