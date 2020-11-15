using System;
using System.Windows;
using System.Windows.Controls;

using GalaSoft.MvvmLight.Messaging;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for SelectUser.xaml
	/// </summary>
	public partial class SelectUser : ContentControl {
		public SelectUser() {
			try {
				InitializeComponent();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}
}
