using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using BibleBooksWPF.Resources;
using BibleBooksWPF.Helpers;
using GalaSoft.MvvmLight.Messaging;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for MainMenu.xaml
	/// </summary>
	public partial class MainMenu : ContentControl {

		public MainMenu() {
			try {
				InitializeComponent();
				LanguageResources.SetDefaultLanguage(this);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
