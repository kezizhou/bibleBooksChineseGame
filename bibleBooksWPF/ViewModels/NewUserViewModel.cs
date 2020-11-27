using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BibleBooksWPF.ViewModels {
	public class NewUserViewModel : INotifyPropertyChanged {

		private string strUsername;
		public string propUsername {
			get {
				return strUsername;
			}
			set {
				strUsername = value;
				NotifyPropertyChanged();
			}
		}

		private profilePic enumProfilePic = profilePic.boy1;
		public profilePic propProfilePic {
			get {
				return enumProfilePic;
			}
			set {
				enumProfilePic = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}
}
