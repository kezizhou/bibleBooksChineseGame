using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace BibleBooksWPF.ViewModels {
	public class UserControlsViewModel : INotifyPropertyChanged {
		private string strPageTitle = ((MainWindow)Application.Current.MainWindow).Content.ToString().Substring(14);

		public string propPageTitle {
			get { 
				return strPageTitle; 
			}
			set {
				strPageTitle = value;
				NotifyPropertyChanged();
			}
		}


		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class MenuConverter : IValueConverter {

		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			return value.ToString() != parameter.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo cultuer) {
			return null;
		}
	}
}
