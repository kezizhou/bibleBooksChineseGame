using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

using BibleBooksWPF.Helpers;
using BibleBooksWPF.Views;
using System.Globalization;
using System.Windows.Data;

namespace BibleBooksWPF.ViewModels {
	public class SelectUserViewModel : INotifyPropertyChanged {
		public RelayCommand<object> deleteUserCommand { get; private set; }
		public RelayCommand<object> selectUserCommand { get; private set; }
		public RelayCommand changeLanguageCommand { get; private set; }

		public SelectUserViewModel() {
			deleteUserCommand = new RelayCommand<object>(DeleteUser);
			selectUserCommand = new RelayCommand<object>(SelectUser);
			changeLanguageCommand = new RelayCommand(ChangeLanguage);

			propRootUser = LoadRootUser();
			UpdateJSONUsers();
		}

		private ObservableCollection<User> _users;
		public ObservableCollection<User> propUsers {
			get { 
				return _users; 
			}
			set {
				_users = value;
				NotifyPropertyChanged();
			}
		}

		private RootUser rootUser;
		public RootUser propRootUser {
			get { 
				return rootUser;
			}
			set { 
				rootUser = value;
				propUsers = new ObservableCollection<User>(rootUser.Users);
				NotifyPropertyChanged();
			}
		}

		private RootUser LoadRootUser() {
			RootUser rootUser = new RootUser();

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
					rootUser = serializer.Deserialize(file, typeof(RootUser)) as RootUser;
				}
			}

			foreach (User user in rootUser.Users.ToArray()) {
				if (user.username == null) {
					rootUser.Users.Remove(user);
				}
			}

			if (rootUser.Users.Count < 3) {
				for(int i = rootUser.Users.Count; i < 3; i++) {
					rootUser.Users.Add(new User());
				}
			}

			return rootUser;
		}

		private Visibility visAbleToDelete = Visibility.Visible;
		public Visibility propAbleToDelete {
			get {
				return visAbleToDelete;
			}
			set { 
				visAbleToDelete = value;
				NotifyPropertyChanged();
			}
		}

		private void DeleteUser(object sender) {
			User user = sender as User;

			// Confirm message box
			ConfirmMessageBox winConfirm = new ConfirmMessageBox($"Are you sure you want to delete user: {user.username}?" + Environment.NewLine + "This cannot be undone.");

			switch (winConfirm.strMsgReturn) {
				case "Yes":
					propRootUser.Users.Remove(user);

					// Update JSON file
					UpdateJSONUsers();

					// Add blank new user to replace deleted
					if (propRootUser.Users.Count < 3) {
						for (int i = rootUser.Users.Count; i < 3; i++) {
							rootUser.Users.Add(new User());
						}
					}
					propUsers = new ObservableCollection<User>(rootUser.Users);
					App.Current.Properties["currentUsername"] = null;

					break;
				default:
					break;
			}
		}

		private void SelectUser(object sender) {
			try {
				User user = sender as User;

				if (user.username == null) {
					NewUser();
				} else {
					// Existing user
					LoadExistingUser(user.username);

					ChangeViewMessage.Navigate("MainMenu");
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void ChangeLanguage() {
			if (Properties.Settings.Default.strLanguage == "en-US") {
				Properties.Settings.Default.strLanguage = "zh-CN";
				Properties.Settings.Default.Save();
			} else if (Properties.Settings.Default.strLanguage == "zh-CN") {
				Properties.Settings.Default.strLanguage = "en-US";
				Properties.Settings.Default.Save();
			}

			RefreshPageMessage.Refresh("SelectUser");
		}

		private void NewUser() {
			NewUser winNewUser = new NewUser();
			winNewUser.ShowDialog();

			if (winNewUser.strMsgReturn == "") {
				NewUserViewModel viewModel = winNewUser.DataContext as NewUserViewModel;
				CreateUserData(viewModel.propUsername, viewModel.propProfilePic.ToString());
				ChangeViewMessage.Navigate("MainMenu");
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

			// Keep this block for those who have dated profiles using English/Chinese rather than language code
			if (userCurrent.strLanguage == "English") {
				userCurrent.strLanguage = "en-US";
				userToken.Replace(JToken.FromObject(userCurrent));

				string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
				File.WriteAllText(Globals.usersFilePath, newJson);
			} else if (userCurrent.strLanguage == "Chinese") {
				userCurrent.strLanguage = "zh-CN";
				userToken.Replace(JToken.FromObject(userCurrent));

				string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
				File.WriteAllText(Globals.usersFilePath, newJson);
			}

			Properties.Settings.Default.strLanguage = userCurrent.strLanguage;

			// Last total points setting
			Properties.Settings.Default.lngTotalPoints = userCurrent.lngTotalPoints;
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
			Properties.Settings.Default.strLanguage = "en-US";
			Properties.Settings.Default.lngTotalPoints = 0;

			propRootUser.Users.Add(currentUser);

			// Add to JSON file
			UpdateJSONUsers();

			propUsers = new ObservableCollection<User>(rootUser.Users);
		}

		private void UpdateJSONUsers() {
			using (StreamWriter file = File.CreateText(Globals.usersFilePath)) {
				JsonSerializer serializer = new JsonSerializer();
				serializer.Formatting = Formatting.Indented;
				serializer.Serialize(file, propRootUser);
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class ProfilePicConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return null;
			}

			return $"pack://application:,,,/BibleBooksWPF;component/Resources/ProfileImages/{value}.png";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}
}
