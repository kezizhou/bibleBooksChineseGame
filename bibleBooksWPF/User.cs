using System.Collections.Generic;
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
