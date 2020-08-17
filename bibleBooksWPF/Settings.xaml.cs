using System.Windows;
using System.Windows.Controls;
using System.Deployment.Application;
using System;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Page {

		public Settings() {
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				// Previous audio setting
				if (Properties.Settings.Default.blnAudio) {
					radAudioOn.IsChecked = true;
					radAudioOff.IsChecked = false;
				} else {
					radAudioOn.IsChecked = false;
					radAudioOff.IsChecked = true;
				}

				// Previous language setting
				if (Properties.Settings.Default.strLanguage.Equals("English")) {
					radEnglish.IsChecked = true;
					radChinese.IsChecked = false;
				} else if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					radEnglish.IsChecked = false;
					radChinese.IsChecked = true;

					// Menu bar
					imenMainMenu.Header = "主菜单";
					imenHebrew.Header = "希伯来语经卷";
					imenMatchHebrew.Header = "中英文配对";
					imenReorderHebrew.Header = "排序";
					imenGreek.Header = "希腊语经卷";
					imenMatchGreek.Header = "中英文配对";
					imenReorderGreek.Header = "排序";
					imenStatistics.Header = "成绩";
					imenSettings.Header = "设置";
					imenExit.Header = "退出";
					menTop.FontSize = 16;

					// Settings descriptions
					grpAudio.Header = "声音";
					radAudioOn.Content = "开";
					radAudioOff.Content = "关";
					grpLanguage.Header = "语言";
					radEnglish.Content = "英文";
					radChinese.Content = "中文";
					grpAbout.Header = "关于游戏";
					txbVersionDesc.Text = "版本";
					btnUpdate.Content = "检查更新";
					grpUserSettings.Header = "用户设置";
					txbUsername.Text = "用户名";
					txbProfilePic.Text = "头像";
					btnSaveSettings.Content = "保存";
					txbSaved.Text = "设置已经储存。";
					grpCredits.Header = "制作";
				}

				// Current version
				if (ApplicationDeployment.IsNetworkDeployed) {
					Version version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
					txbVersion.Text = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Revision);
				} else {
					if (Properties.Settings.Default.strLanguage.Equals("English")) {
						txbVersion.Text = "Not Installed";
					} else if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
						txbVersion.Text = "没有安装";
					}
				}

				// Get user from JSON file
				JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
				JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
				User userCurrent = userToken.ToObject<User>();

				// Load user settings
				txtInputText.Text = userCurrent.username;
				RadioButton radProfilePic = this.FindName(userCurrent.profilePicture) as RadioButton;
				radProfilePic.IsChecked = true;

			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void BtnUpdate_Click(object sender, RoutedEventArgs e) {
			InstallUpdateSyncWithInfo();
		}

		private void InstallUpdateSyncWithInfo() {
			UpdateCheckInfo info = null;

			if (ApplicationDeployment.IsNetworkDeployed) {
				ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

				try {
					info = ad.CheckForDetailedUpdate();

				}
				catch (DeploymentDownloadException dde) {
					MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
					return;
				}
				catch (InvalidDeploymentException ide) {
					MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
					return;
				}
				catch (InvalidOperationException ioe) {
					MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
					return;
				}

				if (info.UpdateAvailable) {
					Boolean doUpdate = true;

					if (!info.IsUpdateRequired) {
						MessageBoxResult msgResult = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButton.OKCancel);
						if (!(MessageBoxResult.OK == msgResult)) {
							doUpdate = false;
						}
					}
					else {
						// Display a message that the app MUST reboot. Display the minimum required version.
						MessageBox.Show( "This application has detected a mandatory update from your current " +
							"version to version " + info.MinimumRequiredVersion.ToString() +
							". The application will now install the update and restart.",
							"Update Available", MessageBoxButton.OK,
							MessageBoxImage.Information );
					}

					if (doUpdate) {
						try {
							ad.Update();
							MessageBox.Show("The application has been upgraded, and will now restart.");
							System.Windows.Forms.Application.Restart();
							Application.Current.Shutdown();
						}
						catch (DeploymentDownloadException dde) {
							MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
							return;
						}
					}
				} else {
					// No updates
					MessageBox.Show("There are no new updates.");
				}
			} else {
				// Program is not installed, debug mode
				MessageBox.Show("The program has not been installed on your computer. Please try and re-install the application.");
				return;
			}
		}

		private void ImenMainMenu_Click(object sender, RoutedEventArgs e) {
			MainMenu pMainMenu = new MainMenu();
			NavigationService.Navigate(pMainMenu);
		}

		private void ImenMatchHebrew_Click(object sender, RoutedEventArgs e) {
			HebrewMatch pHebrewMatch = new HebrewMatch();
			NavigationService.Navigate(pHebrewMatch);
		}

		private void ImenReorderHebrew_Click(object sender, RoutedEventArgs e) {
			HebrewReorder pHebrewReorder = new HebrewReorder();
			NavigationService.Navigate(pHebrewReorder);
		}

		private void ImenMatchGreek_Click(object sender, RoutedEventArgs e) {
			GreekMatch pGreekMatch = new GreekMatch();
			NavigationService.Navigate(pGreekMatch);
		}

		private void ImenReorderGreek_Click(object sender, RoutedEventArgs e) {
			GreekReorder pGreekReorder = new GreekReorder();
			NavigationService.Navigate(pGreekReorder);
		}

		private void ImenStatistics_Click(object sender, RoutedEventArgs e) {
			StatisticsPage pStatistics = new StatisticsPage();
			NavigationService.Navigate(pStatistics);
		}

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void RadAudioOff_Checked(object sender, RoutedEventArgs e) {
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Update the setting in json
			userCurrent.blnAudio = false;
			Properties.Settings.Default.blnAudio = false;
			Properties.Settings.Default.Save();
			userToken.Replace(JToken.FromObject(userCurrent));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText(Globals.usersFilePath, newJson);
		}

		private void RadAudioOn_Checked(object sender, RoutedEventArgs e) {
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Update the setting in json
			userCurrent.blnAudio = true;
			Properties.Settings.Default.blnAudio = true;
			Properties.Settings.Default.Save();
			userToken.Replace(JToken.FromObject(userCurrent));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText(Globals.usersFilePath, newJson);
		}

		private void btnSaveSettings_Click(object sender, RoutedEventArgs e) {
			try {
				// Get user from JSON file
				JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
				JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
				User userCurrent = userToken.ToObject<User>();

				// Update the json with new information
				// Username
				// Check if valid
				if (Properties.Settings.Default.strLanguage.Equals("English")) {
					switch (txtInputText.Text) {
						case "":
							txbSaved.Text = "Please enter a valid username";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
						case "New User":
							txbSaved.Text = "Please enter a valid username";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
						case "新用户":
							txbSaved.Text = "Please enter a valid username";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
						default:
							txbSaved.Text = "Please enter a unique username";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
					}
				} else if (Properties.Settings.Default.strLanguage.Equals("Chinese")) {
					switch (txtInputText.Text) {
						case "":
							txbSaved.Text = "请输入用户名";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
						case "New User":
							txbSaved.Text = "请换一个用户名";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
						case "新用户":
							txbSaved.Text = "请换一个用户名";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
						default:
							txbSaved.Text = "用户名已经存在";
							txbSaved.Foreground = System.Windows.Media.Brushes.Red;
							txbSaved.Visibility = Visibility.Visible;
							AsyncHideSaved();
							return;
					}
				}

				userCurrent.username = txtInputText.Text;
				App.Current.Properties["currentUsername"] = userCurrent.username;

				// Find which profile picture was selected
				if ( wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null ) {
					userCurrent.profilePicture = wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
				} else if ( wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null ) {
					userCurrent.profilePicture = wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
				}

				userToken.Replace(JToken.FromObject(userCurrent));

				string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
				File.WriteAllText(Globals.usersFilePath, newJson);

				txbSaved.Visibility = Visibility.Visible;
				AsyncHideSaved();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private async void AsyncHideSaved() {
			await Task.Delay(1800);
			txbSaved.Visibility = Visibility.Hidden;
		}

		private void radEnglish_Checked(object sender, RoutedEventArgs e) {
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Update the setting in json
			userCurrent.strLanguage = "English";
			Properties.Settings.Default.strLanguage = "English";
			Properties.Settings.Default.Save();
			userToken.Replace(JToken.FromObject(userCurrent));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText(Globals.usersFilePath, newJson);
		}

		private void radChinese_Checked(object sender, RoutedEventArgs e) {
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Update the setting in json
			userCurrent.strLanguage = "Chinese";
			Properties.Settings.Default.strLanguage = "Chinese";
			Properties.Settings.Default.Save();
			userToken.Replace(JToken.FromObject(userCurrent));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText(Globals.usersFilePath, newJson);
		}

		private void RefreshPage() {
			Settings pSettings = new Settings();
			NavigationService.Navigate(pSettings);
		}

		private void radEnglish_Unchecked(object sender, RoutedEventArgs e) {
			RefreshPage();
		}

		private void radChinese_Unchecked(object sender, RoutedEventArgs e) {
			RefreshPage();
		}
	}
}
