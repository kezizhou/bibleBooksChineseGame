using GalaSoft.MvvmLight.CommandWpf;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System;
using System.Windows;

using BibleBooksWPF.Helpers;
using BibleBooksWPF.Views;
using BibleBooksWPF.Resources;
using BibleBooksWPF.Models;

namespace BibleBooksWPF.ViewModels {
	public class MainMenuViewModel : INotifyPropertyChanged {
		public RelayCommand changeUserCommand { get; private set; }
		public RelayCommand helpWindowCommand { get; private set; }
		public RelayCommand pageLoadedCommand { get; private set; }

		public MainMenuViewModel() {
			changeUserCommand = new RelayCommand(ChangeUser);
			helpWindowCommand = new RelayCommand(HelpWindow);
			pageLoadedCommand = new RelayCommand(PageLoaded);
		}

		public void PageLoaded() {
			try {
				List<string> lstBadges = Badge.GetObtainedBadgeNames();
				NewBadgeMessage winBadgeBox = new NewBadgeMessage();

				TimeSpan tsMorning = new TimeSpan(8, 30, 0);
				TimeSpan tsNight = new TimeSpan(21, 0, 0);
				if (DateTime.Now.TimeOfDay < tsMorning && !lstBadges.Contains("imgBadgeMorning")) {
					// Morning badge earned
					Badge.AddBadgeForCurrentUser("imgBadgeMorning");
					CustomMessageBoxMethods.ShowMessage("imgBadgeMorning", winBadgeBox);
				} else if (DateTime.Now.TimeOfDay > tsNight && !lstBadges.Contains("imgBadgeNight")) {
					// Night badge earned
					Badge.AddBadgeForCurrentUser("imgBadgeNight");
					CustomMessageBoxMethods.ShowMessage("imgBadgeNight", winBadgeBox);
				}
				App.Current.Properties["exodusBadge"] = "";
				App.Current.Properties["ruthBadge"] = "";
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

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

		private void ChangeUser() {
			ChangeViewMessage.Navigate("SelectUser");
		}

		private void HelpWindow() {
			HelpWindow helpWindow = new HelpWindow();
			helpWindow.ShowDialog();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}
}
