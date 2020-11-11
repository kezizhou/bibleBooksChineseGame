using BibleBooksWPF.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace BibleBooksWPF.UserControls {
	/// <summary>
	/// Interaction logic for MenuControl.xaml
	/// </summary>
	public partial class MenuControl : UserControl {
		MainWindow mainWindow = Application.Current.MainWindow as MainWindow;
		UserControlsViewModel viewModel;

		public MenuControl() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
		}

		private void UserControl_Loaded(object sender, RoutedEventArgs e) {
			viewModel = new UserControlsViewModel();
			this.DataContext = viewModel;
		}

		private void imenMainMenu_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/MainMenu.xaml", UriKind.Relative));
		}

		private void imenHebrewMatch_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/HebrewMatch.xaml", UriKind.Relative));
		}

		private void imenHebrewReorder_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/HebrewReorder.xaml", UriKind.Relative));
		}

		private void imenGreekReorder_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/GreekReorder.xaml", UriKind.Relative));
		}

		private void imenGreekMatch_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/GreekMatch.xaml", UriKind.Relative));
		}

		private void imenStatistics_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/StatisticsPage.xaml", UriKind.Relative));
		}

		private void imenSettings_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/Settings.xaml", UriKind.Relative));
		}

		private void imenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void imenDiceHebrew_Click(object sender, RoutedEventArgs e) {

		}

		private void imenDiceGreek_Click(object sender, RoutedEventArgs e) {
			mainWindow.Navigate(new Uri("../Views/DiceBooksGreek.xaml", UriKind.Relative));
		}
	}
}
