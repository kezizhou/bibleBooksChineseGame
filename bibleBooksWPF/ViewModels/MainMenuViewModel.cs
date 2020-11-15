using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BibleBooksWPF.ViewModels {
	public class MainMenuViewModel : INotifyPropertyChanged {

		private string strUsername = App.Current.Properties.Contains("currentUsername") ? App.Current.Properties["currentUsername"].ToString() : "";
		public string propUsername {
			get { 
				return strUsername; 
			}
			set { 
				strUsername = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}
}
