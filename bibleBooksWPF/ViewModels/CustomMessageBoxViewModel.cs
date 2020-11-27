using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace BibleBooksWPF.ViewModels {
	public class CustomMessageBoxViewModel : INotifyPropertyChanged {

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

		private string strRecord;
		public string propRecord {
			get { 
				return strRecord; 
			}
			set { 
				strRecord = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class RecordTimeConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return Visibility.Collapsed;
			}

			if (value.ToString() == "time" || value.ToString() == "both") {
				return Visibility.Visible;
			} else {
				return Visibility.Collapsed;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class RecordPointConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return Visibility.Collapsed;
			}

			if (value.ToString() == "point" || value.ToString() == "both") {
				return Visibility.Visible;
			} else {
				return Visibility.Collapsed;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class NewRecordTextConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return Visibility.Collapsed;
			}

			if (value.ToString() == "both") {
				return "New Records!";
			} else {
				return "New Record!";
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class NewRecordVisConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return Visibility.Collapsed;
			} else {
				return Visibility.Visible;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}
}
