using System.Windows;
using System.Windows.Controls;
using System.Deployment.Application;
using System;
using System.Threading.Tasks;
using BibleBooksWPF.ViewModels;
using System.Windows.Navigation;

namespace BibleBooksWPF.Views {
	/// <summary>
	/// Interaction logic for Settings.xaml
	/// </summary>
	public partial class Settings : ContentControl {
		SettingsViewModel viewModel;

		public Settings() {
			InitializeComponent();
			LanguageResources.SetDefaultLanguage(this);
		}

		private void Page_Loaded(object sender, RoutedEventArgs e) {
			try {
				viewModel = new SettingsViewModel();
				this.DataContext = viewModel;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
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
					MessageBox.Show("The new version of the application cannot be downloaded at this time. \n\nPlease check your network connection, or try again later. Error: " + dde.Message);
					return;
				}
				catch (InvalidDeploymentException ide) {
					MessageBox.Show("Cannot check for a new version of the application. The ClickOnce deployment is corrupt. Please redeploy the application and try again. Error: " + ide.Message);
					return;
				}
				catch (InvalidOperationException ioe) {
					MessageBox.Show("This application cannot be updated. It is likely not a ClickOnce application. Error: " + ioe.Message);
					return;
				}

				if (info.UpdateAvailable) {
					Boolean doUpdate = true;

					if (!info.IsUpdateRequired) {
						MessageBoxResult msgResult = MessageBox.Show("An update is available. Would you like to update the application now?", "Update Available", MessageBoxButton.OKCancel);
						if (!(MessageBoxResult.OK == msgResult)) {
							doUpdate = false;
						}
					}
					else {
						// Display a message that the app MUST reboot. Display the minimum required version.
						MessageBox.Show( "This application has detected a mandatory update from your current " +
							"version to version " + info.MinimumRequiredVersion.ToString() +
							". The application will now install the update and restart.",
							"Update Available", MessageBoxButton.OK,
							MessageBoxImage.Information );
					}

					if (doUpdate) {
						try {
							ad.Update();
							MessageBox.Show("The application has been upgraded, and will now restart.");
							System.Windows.Forms.Application.Restart();
							Application.Current.Shutdown();
						}
						catch (DeploymentDownloadException dde) {
							MessageBox.Show("Cannot install the latest version of the application. \n\nPlease check your network connection, or try again later. Error: " + dde);
							return;
						}
					}
				} else {
					// No updates
					MessageBox.Show("There are no new updates.");
				}
			} else {
				// Program is not installed, debug mode
				MessageBox.Show("The program has not been installed on your computer. Please try and re-install the application.");
				return;
			}
		}

		private async void btnSaveSettings_Click(object sender, RoutedEventArgs e) {
			try {
				txbSaved.Visibility = Visibility.Visible;
				await Task.Delay(1800);
				txbSaved.Visibility = Visibility.Hidden;
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		private void radLanguage_Unchecked(object sender, RoutedEventArgs e) {
			//Settings pSettings = new Settings();
			//((MainWindow)Application.Current.MainWindow).Navigate("");
		}
	}
}
