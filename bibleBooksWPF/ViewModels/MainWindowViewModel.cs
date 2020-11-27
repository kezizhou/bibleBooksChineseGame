using GalaSoft.MvvmLight.CommandWpf;
using GalaSoft.MvvmLight.Messaging;
using System;
using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Data;

using BibleBooksWPF.Helpers;

namespace BibleBooksWPF.ViewModels {
	public class MainWindowViewModel : INotifyPropertyChanged {
		private DiceBooksHebrewViewModel diceBooksHebrewVM = new DiceBooksHebrewViewModel();
		private DiceBooksGreekViewModel diceBooksGreekVM = new DiceBooksGreekViewModel();
		private GreekMatchViewModel greekMatchVM = new GreekMatchViewModel();
		private GreekReorderViewModel greekReorderVM = new GreekReorderViewModel();
		private HebrewMatchViewModel hebrewMatchVM = new HebrewMatchViewModel();
		private HebrewReorderViewModel hebrewReorderVM = new HebrewReorderViewModel();
		private MainMenuViewModel mainMenuVM = new MainMenuViewModel();
		private SelectUserViewModel selectUserVM = new SelectUserViewModel();
		private SettingsViewModel settingsVM = new SettingsViewModel();
		private StatisticsPageViewModel statisticsPageVM = new StatisticsPageViewModel();

		public RelayCommand<string> navCommand { get; private set; }

		public MainWindowViewModel() {
			navCommand = new RelayCommand<string>(OnNavigate);
			propCurrentViewModel = selectUserVM;
			Messenger.Default.Register<ChangeViewMessage>(this, (message) => ReceiveMessage(message));
		}

		private void ReceiveMessage(ChangeViewMessage message) {
			OnNavigate(message.strDestination);
		}

		private object objCurrentViewModel;
		public object propCurrentViewModel {
			get { 
				return objCurrentViewModel; 
			}
			set { 
				objCurrentViewModel = value;
				NotifyPropertyChanged();
			}
		}

		private string strPage = "SelectUser";
		public string propPage {
			get {
				return strPage;
			}
			set {
				strPage = value;
				NotifyPropertyChanged();
			}
		}

		private Visibility visShowMenu = Visibility.Collapsed;
		public Visibility propShowMenu {
			get {
				return visShowMenu;
			}
			set { 
				visShowMenu = value;
				NotifyPropertyChanged();
			}
		}

		private void OnNavigate(string strDestination) {
			switch (strDestination) {
				case "DiceBooksHebrew":
					propCurrentViewModel = diceBooksHebrewVM;
					break;
				case "DiceBooksGreek":
					propCurrentViewModel = diceBooksGreekVM;
					break;
				case "GreekMatch":
					propCurrentViewModel = greekMatchVM;
					break;
				case "GreekReorder":
					propCurrentViewModel = greekReorderVM;
					break;
				case "HebrewMatch":
					propCurrentViewModel = hebrewMatchVM;
					break;
				case "HebrewReorder":
					propCurrentViewModel = hebrewReorderVM;
					break;
				case "MainMenu":
					propCurrentViewModel = mainMenuVM;
					break;
				case "SelectUser":
					propCurrentViewModel = selectUserVM;
					break;
				case "Settings":
					propCurrentViewModel = settingsVM;
					break;
				case "Statistics":
					propCurrentViewModel = statisticsPageVM;
					break;
				case "Exit":
					Application.Current.Shutdown();
					break;
				default:
					break;
			}

			propPage = strDestination;

			if (strDestination == "SelectUser") {
				propShowMenu = Visibility.Collapsed;
			} else {
				propShowMenu = Visibility.Visible;
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}

	public class MenuConverter : IValueConverter {
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return value.ToString() != parameter.ToString();
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return null;
		}
	}
}
