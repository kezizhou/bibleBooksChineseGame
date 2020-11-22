using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.ComponentModel;
using System.Deployment.Application;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using GalaSoft.MvvmLight.CommandWpf;
using System.Threading.Tasks;

using BibleBooksWPF.Views;
using BibleBooksWPF.Helpers;

namespace BibleBooksWPF.ViewModels {
	public enum profilePic { boy1, boy2, boy3, boy4, boy5, boy6, boy7, boy8, girl1, girl2, girl3, girl4, girl5, girl6, girl7, girl8 };

	public class SettingsViewModel : INotifyPropertyChanged {
		public RelayCommand checkUpdatesCommand { get; private set; }
		public RelayCommand saveSettingsCommand { get; private set; }

		public SettingsViewModel() {
			checkUpdatesCommand = new RelayCommand(CheckUpdates);
			saveSettingsCommand = new RelayCommand(SaveSettings);
		}

		private bool blnAudioOn = Properties.Settings.Default.blnAudio ? true : false;
		public bool propAudioOn {
			get { 
				return blnAudioOn; 
			}
			set {
				blnAudioOn = value;

				if (blnAudioOn) {
					// Sound on
					blnAudioOff = false;
					Properties.Settings.Default.blnAudio = true;
					Properties.Settings.Default.Save();

					// Get user from JSON file
					JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
					JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
					User userCurrent = userToken.ToObject<User>();

					// Update the setting in json
					userCurrent.blnAudio = true;
					userToken.Replace(JToken.FromObject(userCurrent));

					string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
					File.WriteAllText(Globals.usersFilePath, newJson);

					NotifyPropertyChanged();
				}
			}
		}

		private bool blnAudioOff = Properties.Settings.Default.blnAudio ? false : true;
		public bool propAudioOff {
			get {
				return blnAudioOff;
			}
			set {
				blnAudioOff = value;

				if (blnAudioOff) {
					// Sound off
					blnAudioOn = false;
					Properties.Settings.Default.blnAudio = false;
					Properties.Settings.Default.Save();

					// Get user from JSON file
					JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
					JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
					User userCurrent = userToken.ToObject<User>();

					// Update the setting in json
					userCurrent.blnAudio = false;
					userToken.Replace(JToken.FromObject(userCurrent));

					string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
					File.WriteAllText(Globals.usersFilePath, newJson);

					NotifyPropertyChanged();
				}
			}
		}

		private bool blnEnglish = Properties.Settings.Default.strLanguage == "en-US" ? true : false;
		public bool propEnglish {
			get { 
				return blnEnglish; 
			}
			set {
				try {
					blnEnglish = value;

					if (blnEnglish) {
						// English
						blnChinese = false;

						Properties.Settings.Default.strLanguage = "en-US";
						Properties.Settings.Default.Save();

						// Get user from JSON file
						JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
						User userCurrent = userToken.ToObject<User>();

						// Update the setting in json
						userCurrent.strLanguage = "en-US";
						userToken.Replace(JToken.FromObject(userCurrent));

						string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
						File.WriteAllText(Globals.usersFilePath, newJson);

						LanguageResources.SwitchLanguage((Window)Application.Current.MainWindow, "en-US");

						NotifyPropertyChanged();
						NotifyPropertyChanged("propVersion");
						RefreshPageMessage.Refresh("Settings");
					}
				} catch (Exception ex) {
					MessageBox.Show(ex.ToString());
				}
			}
		}

		private bool blnChinese = Properties.Settings.Default.strLanguage == "zh-CN" ? true : false;
		public bool propChinese {
			get {
				return blnChinese;
			}
			set {
				try {
					blnChinese = value;

					if (blnChinese) {
						// Chinese
						blnEnglish = false;

						Properties.Settings.Default.strLanguage = "zh-CN";
						Properties.Settings.Default.Save();

						// Get user from JSON file
						JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
						JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
						User userCurrent = userToken.ToObject<User>();

						// Update the setting in json
						userCurrent.strLanguage = "zh-CN";
						userToken.Replace(JToken.FromObject(userCurrent));

						string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
						File.WriteAllText(Globals.usersFilePath, newJson);

						LanguageResources.SwitchLanguage((Window)Application.Current.MainWindow, "zh-CN");

						NotifyPropertyChanged();
						NotifyPropertyChanged("propVersion");
						RefreshPageMessage.Refresh("Settings");
					}
				} catch (Exception ex) {
					MessageBox.Show(ex.ToString());
				}
			}
		}

		private string strUsername = App.Current.Properties.Contains("currentUsername") ? App.Current.Properties["currentUsername"].ToString() : "";
		public string propUsername {
			get { 
				return strUsername; 
			}
			set {
				strUsername = value;
				User.UpdateUsernameInJson(strUsername);
				App.Current.Properties["currentUsername"] = strUsername;
				NotifyPropertyChanged();
			}
		}

		private profilePic strProfilePic = App.Current.Properties.Contains("currentUsername") ? (profilePic)Enum.Parse(typeof(profilePic), User.GetProfilePicture()) : new profilePic();

		public profilePic propProfilePic {
			get { 
				return strProfilePic; 
			}
			set { 
				strProfilePic = value;
				User.UpdateProfilePicture(strProfilePic.ToString());
				NotifyPropertyChanged();
			}
		}

		private string strVersion;
		public string propVersion {
			get {
				if (ApplicationDeployment.IsNetworkDeployed) {
					Version version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
					strVersion = string.Format("{0}.{1}.{2}", version.Major, version.Minor, version.Revision);
				} else {
					if (Properties.Settings.Default.strLanguage.Equals("en-US")) {
						strVersion = "Not Installed";
					} else if (Properties.Settings.Default.strLanguage.Equals("zh-CN")) {
						strVersion = "没有安装";
					}
				}

				return strVersion; 
			}
			set {
				strVersion = value;
				NotifyPropertyChanged();
			}
		}

		private Visibility blnVisSettingsSaved = Visibility.Collapsed;
		public Visibility propVisSettingsSaved {
			get { 
				return blnVisSettingsSaved; 
			}
			set {
				blnVisSettingsSaved = value;
				NotifyPropertyChanged();
			}
		}

		public async void SaveSettings() {
			try {
				propVisSettingsSaved = Visibility.Visible;
				await Task.Delay(1900);
				propVisSettingsSaved = Visibility.Collapsed;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		public void CheckUpdates() {
			UpdateCheckInfo info = null;

			if (ApplicationDeployment.IsNetworkDeployed) {
				ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

				try {
					info = ad.CheckForDetailedUpdate();
				} catch (DeploymentDownloadException dde) {
					MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
					return;
				} catch (InvalidDeploymentException ide) {
					MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
					return;
				} catch (InvalidOperationException ioe) {
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
					} else {
						// Display a message that the app MUST reboot. Display the minimum required version.
						MessageBox.Show("This application has detected a mandatory update from your current " +
							"version to version " + info.MinimumRequiredVersion.ToString() +
							". The application will now install the update and restart.",
							"Update Available", MessageBoxButton.OK,
							MessageBoxImage.Information);
					}

					if (doUpdate) {
						try {
							ad.Update();
							MessageBox.Show("The application has been upgraded, and will now restart.");
							System.Windows.Forms.Application.Restart();
							Application.Current.Shutdown();
						} catch (DeploymentDownloadException dde) {
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

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class EnumBooleanConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value.Equals(parameter);
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return ((bool)value) ? parameter : Binding.DoNothing;
		}
	}

	public class UsernameValidationRule : ValidationRule {
		public UsernameValidationRule() {

		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			string strInput = value.ToString();

			if (strInput == "") {
				if (Properties.Settings.Default.strLanguage == "en-US") {
					return new ValidationResult(false, "Please enter a valid username");
				} else if (Properties.Settings.Default.strLanguage == "zh-CN") {
					return new ValidationResult(false, "请换一个用户名");
				}
			}
			foreach (string strExistingUsername in User.GetAllUsernames()) {
				if (strInput == strExistingUsername) {
					if (Properties.Settings.Default.strLanguage == "en-US") {
						return new ValidationResult(false, "Please enter a unique username");
					} else if (Properties.Settings.Default.strLanguage == "zh-CN") {
						return new ValidationResult(false, "用户名已经存在");
					}
				}
			}

			return ValidationResult.ValidResult;
		}
	}
}
