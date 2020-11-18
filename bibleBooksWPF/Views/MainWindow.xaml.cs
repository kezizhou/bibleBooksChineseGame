using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow {
		public MainWindow() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
		}

		protected override void OnClosed(EventArgs e) {
			// Save user total points
			// Get user from JSON file
			if (App.Current.Properties["currentUsername"] != null) {

				JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
				JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
				User userCurrent = userToken.ToObject<User>();

				// Update the setting in json
				userCurrent.lngTotalPoints = Properties.Settings.Default.lngTotalPoints;
				Properties.Settings.Default.Save();
				userToken.Replace(JToken.FromObject(userCurrent));

				string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
				File.WriteAllText(Globals.usersFilePath, newJson);
			}

			base.OnClosed(e);
			Application.Current.Shutdown();
		}
	}

	public static class Globals {
		public static string usersFilePath = "";

		static Globals() {
			usersFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "BibleBooksGame", "users.json");
		}
	}
}
