using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

namespace BibleBooksWPF.ViewModels {
	public class NewBadgeMessageViewModel : INotifyPropertyChanged {

		private string strBadgeName;
		public string propBadgeName {
			get { 
				return strBadgeName; 
			}
			set {
				strBadgeName = value;
				NotifyPropertyChanged();
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class BadgeDescConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) {
				return null;
			}

			string strValue = value.ToString().Substring(3);
			if (Properties.Settings.Default.strLanguage == "en-US") {
				ResourceDictionary dictionary = new ResourceDictionary { Source = new Uri("pack://application:,,,/BibleBooksWPF;component/Languages/StatisticsPage.en-US.xaml") };
				return dictionary[strValue];
			} else if (Properties.Settings.Default.strLanguage == "zh-CN") {
				ResourceDictionary dictionary = new ResourceDictionary { Source = new Uri("pack://application:,,,/BibleBooksWPF;component/Languages/StatisticsPage.zh-CN.xaml") };
				return dictionary[strValue];
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}
}
