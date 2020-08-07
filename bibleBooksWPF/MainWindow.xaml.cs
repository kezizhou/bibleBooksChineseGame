using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Windows;
using System.Windows.Navigation;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : NavigationWindow {

		public MainWindow() {
			InitializeComponent();
		}

		protected override void OnClosed(EventArgs e) {
			// Save user total points
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText("users.json"));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Update the setting in json
			userCurrent.lngTotalPoints = Properties.Settings.Default.lngTotalPoints;
			Properties.Settings.Default.Save();
			userToken.Replace(JToken.FromObject(userCurrent));

			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText("users.json", newJson);

			base.OnClosed(e);
			Application.Current.Shutdown();
		}
	}
}
