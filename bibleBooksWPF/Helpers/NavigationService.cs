using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace BibleBooksWPF.Helpers {
	public class NavigationService : INavigationService {
		private MainWindow mainWindow;

		public void NavigateTo(Uri uriPage) {
			if (EnsureMainWindow()) mainWindow.Navigate(uriPage);
		}

		private bool EnsureMainWindow() {
			if (mainWindow != null) return true;

			mainWindow = Application.Current.MainWindow as MainWindow;
			return mainWindow != null;
		}
	}
}
