using ExtensionMethods;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
				// Language settings
				if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					txbSelect.Text = "请选择用户:";
					btnUser1.Content = "新用户";
					btnUser2.Content = "新用户";
					btnUser3.Content = "新用户";
					btnLanguage.Content = "English";
				}

				// Create app directory and users file if they do not exist
				string appDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BibleBooksGame");
				if (!Directory.Exists(appDirectory)) {
					Directory.CreateDirectory(appDirectory);
				}

				if (!File.Exists(Globals.usersFilePath)) {
					File.Create(Globals.usersFilePath);
				}

				// Get the users from the file if not empty
				if (new FileInfo(Globals.usersFilePath).Length != 0) {
					using (StreamReader file = File.OpenText(Globals.usersFilePath)) {
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
				if (btnUser1.Content.Equals("New User") || btnUser1.Content.Equals("新用户")) {
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
				if (btnUser2.Content.Equals("New User") || btnUser2.Content.Equals("新用户")) {
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
				if (btnUser3.Content.Equals("New User") || btnUser3.Content.Equals("新用户")) {
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
			newUser.Item1.Equals("New User") ||								// Is "New User"
			newUser.Item1.Equals("新用户") )									// Is "新用户“
			&& !newUser.Item1.Equals("Cancel") ) {                          // And is not the cancel button

				if (Properties.Settings.Default.strLanguage.Equals("English")) {
					switch (newUser.Item1) {
						case "":
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "Please enter a valid username");
							break;
						case "New User":
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "Please enter a valid username");
							break;
						case "新用户":
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "Please enter a valid username");
							break;
						default:
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "Please enter a unique username");
							break;
					}
				} else if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					switch (newUser.Item1) {
						case "":
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "请输入用户名");
							break;
						case "New User":
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "请换一个用户名");
							break;
						case "新用户":
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "请换一个用户名");
							break;
						default:
							winNewUser = new NewUser();
							newUser = CustomMessageBoxMethods.ShowMessage(winNewUser, "用户名已经存在");
							break;
					}
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
			Properties.Settings.Default.blnAudio = true;
			Properties.Settings.Default.strLanguage = "English";

			lstUsers.Users.Add(currentUser);

			// Add to JSON file
			using (StreamWriter file = File.CreateText(Globals.usersFilePath)) {
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, lstUsers);
			}

		}

		private void LoadExistingUser(string strUsername) {
			App.Current.Properties["currentUsername"] = strUsername;

			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Previous audio and language settings
			Properties.Settings.Default.blnAudio = userCurrent.blnAudio;
			Properties.Settings.Default.strLanguage = userCurrent.strLanguage;
			// Last total points setting
			Properties.Settings.Default.lngTotalPoints = userCurrent.lngTotalPoints;
		}

		private void btnDelete1_Click(object sender, RoutedEventArgs e) {
			// Confirm message box
			ConfirmMessageBox winConfirm = new ConfirmMessageBox();
			string strText = "Are you sure you want to delete user: " + lstUsers.Users[0].username + "?" + System.Environment.NewLine + "This cannot be undone.";
			string strResult = CustomMessageBoxMethods.ShowMessage(strText, winConfirm);

			switch (strResult) {
				case "Yes":
					lstUsers.Users.RemoveAt(0);

					// Update JSON file
					using (StreamWriter file = File.CreateText(Globals.usersFilePath)) {
						JsonSerializer serializer = new JsonSerializer();
						serializer.Formatting = Formatting.Indented;
						serializer.Serialize(file, lstUsers);
					}
					break;
				default:
					break;
			}

			RefreshPage();
		}

		private void btnDelete2_Click(object sender, RoutedEventArgs e) {
			// Confirm message box
			ConfirmMessageBox winConfirm = new ConfirmMessageBox();
			string strText = "Are you sure you want to delete user: " + lstUsers.Users[1].username + "?" + System.Environment.NewLine + "This cannot be undone.";
			string strResult = CustomMessageBoxMethods.ShowMessage(strText, winConfirm);

			switch (strResult) {
				case "Yes":
					lstUsers.Users.RemoveAt(1);

					// Update JSON file
					using (StreamWriter file = File.CreateText(Globals.usersFilePath)) {
						JsonSerializer serializer = new JsonSerializer();
						serializer.Formatting = Formatting.Indented;
						serializer.Serialize(file, lstUsers);
					}
					break;
				default:
					break;
			}
			RefreshPage();
		}

		private void btnDelete3_Click(object sender, RoutedEventArgs e) {
			// Confirm message box
			ConfirmMessageBox winConfirm = new ConfirmMessageBox();
			string strText = "Are you sure you want to delete user: " + lstUsers.Users[2].username + "?" + System.Environment.NewLine + "This cannot be undone.";
			string strResult = CustomMessageBoxMethods.ShowMessage(strText, winConfirm);

			switch (strResult) {
				case "Yes":
					lstUsers.Users.RemoveAt(2);

					// Update JSON file
					using (StreamWriter file = File.CreateText(Globals.usersFilePath)) {
						JsonSerializer serializer = new JsonSerializer();
						serializer.Formatting = Formatting.Indented;
						serializer.Serialize(file, lstUsers);
					}
					break;
				default:
					break;
			}

			RefreshPage();
		}

		private void RefreshPage() {
			SelectUser pSelectUser = new SelectUser();
			NavigationService.Navigate(pSelectUser);
		}

		private void btnLanguage_Click(object sender, RoutedEventArgs e) {
			if (Properties.Settings.Default.strLanguage == "English") {
				Properties.Settings.Default.strLanguage = "Chinese";
				Properties.Settings.Default.Save();
			} else if (Properties.Settings.Default.strLanguage == "Chinese") {
				Properties.Settings.Default.strLanguage = "English";
				Properties.Settings.Default.Save();
			}

			RefreshPage();
		}
	}
}
