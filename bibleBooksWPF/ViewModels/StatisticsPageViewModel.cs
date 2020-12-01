using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using BibleBooksWPF.Models;
using GalaSoft.MvvmLight.CommandWpf;

namespace BibleBooksWPF.ViewModels {
	public class StatisticsPageViewModel : INotifyPropertyChanged {

		public RelayCommand submitCodeCommand { get; private set; }

		public static string[] astrAnimals = new string[]{ 
			"Bear", "Butterfly", "Cat", "Cow", "Dolphin", "Dog", "Elephant", "Fox", "Flamingo",
			"Giraffe", "Horse", "Koala", "Lion", "Panda", "Penguin", "Snake", "Tiger", "Turtle", "Zebra" 
		};

		private ObservableCollection<BaseStatistic> lstHebrewBaseStatistics;
		public ObservableCollection<BaseStatistic> propHebrewBaseStatistics {
			get { 
				return lstHebrewBaseStatistics; 
			}
			set {
				lstHebrewBaseStatistics = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<BaseStatistic> lstGreekBaseStatistics;
		public ObservableCollection<BaseStatistic> propGreekBaseStatistics {
			get {
				return lstGreekBaseStatistics;
			}
			set {
				lstGreekBaseStatistics = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<Badge> lstAllBadges;
		public ObservableCollection<Badge> propAllBadges {
			get {
				lstAllBadges = new ObservableCollection<Badge>(propObtainedBadges.Union(Badge.GetBadgesRequiredToShow()));
				return lstAllBadges; 
			}
			set { 
				lstAllBadges = value;
				NotifyPropertyChanged();
			}
		}

		private ObservableCollection<Badge> lstObtainedBadges;
		public ObservableCollection<Badge> propObtainedBadges {
			get {
				lstObtainedBadges = new ObservableCollection<Badge>(Badge.GetBadgeObjectList());
				return lstObtainedBadges;
			}
			set { 
				lstObtainedBadges = value;
				NotifyPropertyChanged();
			}
		}

		private long lngTotalPoints = Properties.Settings.Default.lngTotalPoints;
		public long propTotalPoints {
			get { 
				return lngTotalPoints;
			}
			set { 
				lngTotalPoints = value;
				Properties.Settings.Default.lngTotalPoints = lngTotalPoints;
				NotifyPropertyChanged();
			}
		}

		private string strCode;
		public string propCode {
			get { 
				return strCode; 
			}
			set { 
				strCode = value;
				NotifyPropertyChanged();
			}
		}

		public StatisticsPageViewModel() {
			propHebrewBaseStatistics = new ObservableCollection<BaseStatistic>(GetHebrewBaseStatistics());
			propGreekBaseStatistics = new ObservableCollection<BaseStatistic>(GetGreekBaseStatistics());
			submitCodeCommand = new RelayCommand(SubmitCode);
		}

		private void SubmitCode() {
			string strFirstCapitalized = char.ToUpper(propCode[0]) + propCode.Substring(1).ToLower();

			if (Badge.lstAnimalBadges.Any(b => b == ("imgBadge" + strFirstCapitalized))) {
				// Animal badges
				Badge.AddBadgeForCurrentUser("imgBadge" + strFirstCapitalized);
				NotifyPropertyChanged("propAllBadges");

				propCode = "";
			}
		}

		private List<BaseStatistic> GetHebrewBaseStatistics() {
			List<BaseStatistic> lstHebrewBaseStatistics = new List<BaseStatistic>();

			lstHebrewBaseStatistics.Add(BaseStatistic.GetBaseStatistic("Matching", "HebrewMatch"));
			lstHebrewBaseStatistics.Add(BaseStatistic.GetBaseStatistic("ReorderStatistic", "HebrewReorder"));

			return lstHebrewBaseStatistics;
		}

		private List<BaseStatistic> GetGreekBaseStatistics() {
			List<BaseStatistic> lstGreekBaseStatistics = new List<BaseStatistic>();

			lstGreekBaseStatistics.Add(BaseStatistic.GetBaseStatistic("Matching", "GreekMatch"));
			lstGreekBaseStatistics.Add(BaseStatistic.GetBaseStatistic("ReorderStatistic", "GreekReorder"));

			return lstGreekBaseStatistics;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class BadgeImgConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) return null;

			if (StatisticsPageViewModel.astrAnimals.Any(a => value.ToString().Contains(a))) {
				return $"pack://application:,,,/BibleBooksWPF;component/Resources/Badges/AnimalBadges/{value}.png";
			}

			if (value.ToString().Contains("Special")) {
				return $"pack://application:,,,/BibleBooksWPF;component/Resources/Badges/SpecialBadges/{value}.png";
			}

			return $"pack://application:,,,/BibleBooksWPF;component/Resources/Badges/{value}.png";
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class LanguageTextConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) return null;

			if (Properties.Settings.Default.strLanguage == "en-US") {
				ResourceDictionary dictionary = new ResourceDictionary { Source = new Uri("pack://application:,,,/BibleBooksWPF;component/Languages/StatisticsPage.en-US.xaml") };
				return dictionary[value];
			} else if (Properties.Settings.Default.strLanguage == "zh-CN") {
				ResourceDictionary dictionary = new ResourceDictionary { Source = new Uri("pack://application:,,,/BibleBooksWPF;component/Languages/StatisticsPage.zh-CN.xaml") };
				return dictionary[value];
			}

			return null;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class ToolTipVisibilityConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value.ToString() == "") {
				return Visibility.Collapsed;
			} else {
				return Visibility.Visible;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class BadgeOpacityConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture) {
			if (value == null) return null;

			if ((bool)value == true) {
				return 1;
			} else {
				return 0.1;
			}
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) {
			return null;
		}
	}

	public class CodeValidationRule : ValidationRule {
		public CodeValidationRule() {

		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo) {
			string strInput = value.ToString();
			string strFirstCapitalized;

			if (strInput.Length == 0 || strInput.Length == 1) {
				// Empty string or 1 character
				return new ValidationResult(false, "Invalid code entered.");
			} else {
				strFirstCapitalized = char.ToUpper(strInput[0]) + strInput.Substring(1).ToLower();
			}
			
			if (Badge.lstAnimalBadges.Any(b => b == ("imgBadge" + strFirstCapitalized))) {
				return ValidationResult.ValidResult;
			}

			// Invalid code
			return new ValidationResult(false, "Invalid code entered.");
		}
	}
}
