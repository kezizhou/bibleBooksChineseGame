using System.Collections.Generic;
using System.Linq;

namespace BibleBooksWPF {
    class User {

		public string username { get; set; }
		public string profilePicture { get; set; }
		public RootGameStatistics lstGameStatistics { get; set; }
		public List<string> lstBadges { get; set; }

		public User(string username, string profilePicture, RootGameStatistics lstGameStatistics, List<string> lstBadges) {
			this.username = username;
			this.profilePicture = profilePicture;
			this.lstGameStatistics = lstGameStatistics;
			this.lstBadges = lstBadges;
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
