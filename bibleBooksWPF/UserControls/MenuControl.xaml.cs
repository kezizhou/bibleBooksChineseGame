using BibleBooksWPF.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
			MainMenu pMainMenu = new MainMenu();
			mainWindow.Navigate(pMainMenu);
		}

		private void imenHebrewMatch_Click(object sender, RoutedEventArgs e) {
			HebrewMatch pHebrewMatch = new HebrewMatch();
			mainWindow.Navigate(pHebrewMatch);
		}

		private void imenHebrewReorder_Click(object sender, RoutedEventArgs e) {
			HebrewReorder pHebrewReorder = new HebrewReorder();
			mainWindow.Navigate(pHebrewReorder);
		}

		private void imenGreekReorder_Click(object sender, RoutedEventArgs e) {
			GreekReorder pGreekReorder = new GreekReorder();
			mainWindow.Navigate(pGreekReorder);
		}

		private void imenGreekMatch_Click(object sender, RoutedEventArgs e) {
			GreekMatch pGreekMatch = new GreekMatch();
			mainWindow.Navigate(pGreekMatch);
		}

		private void imenStatistics_Click(object sender, RoutedEventArgs e) {
			StatisticsPage pStatistics = new StatisticsPage();
			mainWindow.Navigate(pStatistics);
		}

		private void imenSettings_Click(object sender, RoutedEventArgs e) {
			Settings pSettings = new Settings();
			mainWindow.Navigate(pSettings);
		}

		private void imenExit_Click(object sender, RoutedEventArgs e) {
			Application.Current.Shutdown();
		}

		private void imenDiceHebrew_Click(object sender, RoutedEventArgs e) {

		}

		private void imenDiceGreek_Click(object sender, RoutedEventArgs e) {
			DiceBooksGreek pDiceBooksGreek = new DiceBooksGreek();
			mainWindow.Navigate(pDiceBooksGreek);
		}
	}
}
