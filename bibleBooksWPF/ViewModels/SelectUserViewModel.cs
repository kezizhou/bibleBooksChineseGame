using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BibleBooksWPF.ViewModels {
	class SelectUserViewModel : INotifyPropertyChanged {
		private List<User> lstUsers;

		public List<User> propUsers {
			get { 
				return lstUsers; 
			}
			set {
				lstUsers = value;
				NotifyPropertyChanged();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}
}
