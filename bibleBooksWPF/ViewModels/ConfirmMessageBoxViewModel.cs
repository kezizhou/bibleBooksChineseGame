using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BibleBooksWPF.ViewModels {
	public class ConfirmMessageBoxViewModel : INotifyPropertyChanged {
		private string strMessage;
		public string propMessage {
			get { 
				return strMessage; 
			}
			set { 
				strMessage = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}
}
