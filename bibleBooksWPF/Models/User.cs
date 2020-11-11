using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace BibleBooksWPF {
    class User {
		public string username { get; set; }
		public string profilePicture { get; set; }
		public long lngTotalPoints { get; set; }
		public bool blnAudio { get; set; }
		public string strLanguage { get; set; }
		public RootGameStatistics lstGameStatistics { get; set; }
		public List<string> lstBadges { get; set; }

		public User(string username, string profilePicture, long lngTotalPoints, bool blnAudio, string strLanguage, RootGameStatistics lstGameStatistics, List<string> lstBadges) {
			this.username = username;
			this.profilePicture = profilePicture;
			this.lngTotalPoints = lngTotalPoints;
			this.blnAudio = blnAudio;
			this.strLanguage = strLanguage;
			this.lstGameStatistics = lstGameStatistics;
			this.lstBadges = lstBadges;
		}

		public User(string username, string profilePicture, RootGameStatistics lstGameStatistics, List<string> lstBadges) {
			this.username = username;
			this.profilePicture = profilePicture;
			this.lstGameStatistics = lstGameStatistics;
			this.lstBadges = lstBadges;

			// Default
			this.lngTotalPoints = 0;
			this.blnAudio = true;
			this.strLanguage = "English";
		}

		public User() {

		}

		public static void UpdateUsernameInJson(string strNewUsername) {
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Update the json with new information
			userCurrent.username = strNewUsername;
			userToken.Replace(JToken.FromObject(userCurrent));
			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText(Globals.usersFilePath, newJson);
		}

		public static void UpdateProfilePicture(string strNewPic) {
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Update the json with new information
			userCurrent.profilePicture = strNewPic;
			userToken.Replace(JToken.FromObject(userCurrent));
			string newJson = JsonConvert.SerializeObject(obj, Formatting.Indented);
			File.WriteAllText(Globals.usersFilePath, newJson);
		}

		public static string GetProfilePicture() {
			// Get user from JSON file
			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userToken = obj.SelectToken("$.Users[?(@.username == '" + App.Current.Properties["currentUsername"] + "')]");
			User userCurrent = userToken.ToObject<User>();

			// Load user settings
			return userCurrent.profilePicture;
		}
	}

	class RootUser {
		public List<User> Users { get; set; }

		public RootUser(IEnumerable<User> Users) {
			this.Users = Users.ToList();
		}

		public RootUser() {
			this.Users = new List<User>();
		}
	}
}
