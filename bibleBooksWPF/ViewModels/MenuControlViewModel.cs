using BibleBooksWPF.Helpers;
using BibleBooksWPF.Models;
using GalaSoft.MvvmLight.CommandWpf;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BibleBooksWPF.ViewModels {
	public class MenuControlViewModel : INotifyPropertyChanged {
		private readonly INavigationService _navigationService;
		MenuControlViewModel(INavigationService navigationService) {
			_navigationService = navigationService;
		}

		private RelayCommand<MenuControlModel> _navigate;
		public RelayCommand<MenuControlModel> NavigateToMain {
			get {
				return _navigate ?? (_navigate = new RelayCommand<MenuControlModel>(
									navigate => {
										_navigationService.NavigateTo(new Uri("/MainMeu.xaml", UriKind.Relative));
									}));
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}
}
