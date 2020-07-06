﻿using System.Windows;
using System.Windows.Controls;
using System.Deployment.Application;
using System;
using System.Windows.Forms;

namespace BibleBooksWPF {
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : Page {
		public Settings() {
			InitializeComponent();
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			if (ApplicationDeployment.IsNetworkDeployed) {
				Version version = ApplicationDeployment.CurrentDeployment.CurrentVersion;
				lblVersion.Text = string.Format("{0}.{1}.{2}.{3}", version.Major, version.Minor, version.Build, version.Revision);
			} else {
				lblVersion.Text = "Not Installed";
			}
		}

		private void RadAudio_IsEnabledChanged(object sender, DependencyPropertyChangedEventArgs e) {
			if (radAudioOn.IsChecked == true) {
				App.Current.Properties["blnAudio"] = true;
			}
			else {
				App.Current.Properties["blnAudio"] = false;
			}
		}

		private void BtnUpdate_Click(object sender, RoutedEventArgs e) {
			InstallUpdateSyncWithInfo();
		}

		private void InstallUpdateSyncWithInfo() {
			UpdateCheckInfo info = null;

			if (ApplicationDeployment.IsNetworkDeployed) {
				ApplicationDeployment ad = ApplicationDeployment.CurrentDeployment;

				try {
					info = ad.CheckForDetailedUpdate();

				}
				catch (DeploymentDownloadException dde) {
					System.Windows.MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
					return;
				}
				catch (InvalidDeploymentException ide) {
					System.Windows.MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
					return;
				}
				catch (InvalidOperationException ioe) {
					System.Windows.MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
					return;
				}

				if (info.UpdateAvailable) {
					Boolean doUpdate = true;

					if (!info.IsUpdateRequired) {
						MessageBoxResult msgResult = System.Windows.MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButton.OKCancel);
						if (!(MessageBoxResult.OK == msgResult)) {
							doUpdate = false;
						}
					}
					else {
						// Display a message that the app MUST reboot. Display the minimum required version.
						System.Windows.MessageBox.Show( "This application has detected a mandatory update from your current " +
							"version to version " + info.MinimumRequiredVersion.ToString() +
							". The application will now install the update and restart.",
							"Update Available", MessageBoxButton.OK,
							MessageBoxImage.Information );
					}

					if (doUpdate) {
						try {
							ad.Update();
							System.Windows.MessageBox.Show("The application has been upgraded, and will now restart.");
							System.Windows.Forms.Application.Restart();
							System.Windows.Application.Current.Shutdown();
						}
						catch (DeploymentDownloadException dde) {
							System.Windows.MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
							return;
						}
					}
				}
			} else {
				// Program is not installed, debug mode
				System.Windows.MessageBox.Show("The program has not been installed on your computer. Please try and re-install the application.");
				return;
			}
		}

		private void ImenMainMenu_Click(object sender, RoutedEventArgs e) {
			MainMenu pMainMenu = new MainMenu();
			NavigationService.Navigate(pMainMenu);
		}

		private void ImenMatchHebrew_Click(object sender, RoutedEventArgs e) {
			HebrewMatch pHebrewMatch = new HebrewMatch();
			NavigationService.Navigate(pHebrewMatch);
		}

		private void ImenMatchGreek_Click(object sender, RoutedEventArgs e) {
			GreekMatch pGreekMatch = new GreekMatch();
			NavigationService.Navigate(pGreekMatch);
		}

		private void ImenReorderGreek_Click(object sender, RoutedEventArgs e) {
			GreekReorder pGreekReorder = new GreekReorder();
			NavigationService.Navigate(pGreekReorder);
		}

		private void ImenExit_Click(object sender, RoutedEventArgs e) {
			System.Windows.Application.Current.Shutdown();
		}
	}
}
