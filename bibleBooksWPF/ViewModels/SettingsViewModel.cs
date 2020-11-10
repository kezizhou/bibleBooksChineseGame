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

namespace BibleBooksWPF.ViewModels {
	public enum profilePic { boy1, boy2, boy3, boy4, boy5, boy6, boy7, boy8, girl1, girl2, girl3, girl4, girl5, girl6, girl7, girl8 };

	class SettingsViewModel : INotifyPropertyChanged {
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

						LanguageResources.SwitchLanguage((Page)Application.Current.MainWindow.Content, "en-US");

						NotifyPropertyChanged();
						NotifyPropertyChanged("propVersion");
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

						LanguageResources.SwitchLanguage((Page)Application.Current.MainWindow.Content, "zh-CN");

						NotifyPropertyChanged();
						NotifyPropertyChanged("propVersion");
					}
				} catch (Exception ex) {
					MessageBox.Show(ex.ToString());
				}
			}
		}

		private string strUsername = App.Current.Properties["currentUsername"].ToString();
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

		private profilePic strProfilePic = (profilePic)Enum.Parse(typeof(profilePic), User.GetProfilePicture());

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

			if (strInput == "" || strInput == "New User" || strInput == "新用户") {
				return new ValidationResult(false, "Please enter a unique username");
			}

			return ValidationResult.ValidResult;
		}
	}
}
