using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace BibleBooksWPF.Helpers {
	public static class CustomMessageBoxMethods {
		public static string ShowMessage(string strMessage, string strTitle, string strIcon, CustomMessageBox winMsgBox) {
			string strResult = "";

			try {
				// Message text 
				TextBlock txb = winMsgBox.FindName("txbMessageText") as TextBlock;
				txb.Text = strMessage;

				// Title
				winMsgBox.Title = strTitle;

				// Icon
				Image img = winMsgBox.FindName("imgIcon") as Image;

				// Show correct icon according to parameter
				switch (strIcon) {
					case "congrats":
						img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/congrats.png"));
						break;
					default:
						break;
				}

				// Pop-up message box and get return value
				winMsgBox.ShowDialog();
				if (winMsgBox.strMsgReturn == null) {
					strResult = "Main";
				} else {
					strResult = winMsgBox.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, string strRecord, CustomMessageBox winMsgBox) {
			string strResult = "";

			try {
				// Message text 
				TextBlock txb = winMsgBox.FindName("txbMessageText") as TextBlock;
				txb.Text = strMessage;

				// Title
				winMsgBox.Title = strTitle;

				// Icon
				Image img = winMsgBox.FindName("imgIcon") as Image;

				// Show correct icon according to parameter
				switch (strIcon) {
					case "congrats":
						img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/congrats.png"));
						break;
					default:
						break;
				}

				// Show record message
				Image imgTime = winMsgBox.FindName("imgTimeRecord") as Image;
				Image imgPoint = winMsgBox.FindName("imgPointRecord") as Image;
				TextBlock txbRecord = winMsgBox.FindName("txbRecord") as TextBlock;
				switch (strRecord) {
					case "both":
						imgTime.Visibility = Visibility.Visible;
						imgPoint.Visibility = Visibility.Visible;
						txbRecord.Text = "New records!";
						txbRecord.Visibility = Visibility.Visible;
						break;
					case "time":
						imgTime.Visibility = Visibility.Visible;
						txbRecord.Visibility = Visibility.Visible;
						break;
					case "point":
						imgPoint.Visibility = Visibility.Visible;
						txbRecord.Visibility = Visibility.Visible;
						break;
					default:
						break;
				}

				// Pop-up message box and get return value
				winMsgBox.ShowDialog();
				if (winMsgBox.strMsgReturn == null) {
					strResult = "Main";
				} else {
					strResult = winMsgBox.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, CustomInputBox winInputBox) {
			string strResult = "";

			try {
				// Message text 
				TextBlock txb = winInputBox.FindName("txbMessageText") as TextBlock;
				txb.Text = strMessage;

				// Title
				winInputBox.Title = strTitle;

				// Icon
				Image img = winInputBox.FindName("imgIcon") as Image;

				// Show correct icon according to parameter
				switch (strIcon) {
					case "newUser":
						img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/newuser.png"));
						break;
					default:
						break;
				}

				// Input text
				TextBox txt = winInputBox.FindName("txtInputText") as TextBox;

				// Pop-up message box
				winInputBox.ShowDialog();

				// Get return value
				if (winInputBox.strMsgReturn == null) {
					// Return cancel string
					strResult = "Cancel";
				} else if (winInputBox.strMsgReturn.Equals("")) {
					// Username was entered, return username
					strResult = txt.Text;
				} else {
					// Return cancel string
					strResult = winInputBox.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static void ShowMessage(string strBadgeName, NewBadgeMessage winBadgeBox) {
			try {
				// Set image to new badge
				Image img = winBadgeBox.FindName("imgBadge") as Image;
				img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/Badges/" + strBadgeName + ".png"));

				// Set description of badge
				TextBlock txbDescription = winBadgeBox.FindName("txbDescription") as TextBlock;
				StatisticsPage pStatistics = new StatisticsPage();
				Image imgBadge = pStatistics.FindName(strBadgeName) as Image;
				txbDescription.Text += imgBadge.ToolTip.ToString();

				// Pop-up message box
				winBadgeBox.ShowDialog();
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}

		public static string ShowMessage(string strMessage, string strTitle, string strIcon, string strValidation, CustomInputBox winInputBox) {
			string strResult = "";

			try {
				// Message text 
				TextBlock txb = winInputBox.FindName("txbMessageText") as TextBlock;
				txb.Text = strMessage;

				// Title
				winInputBox.Title = strTitle;

				// Icon
				Image img = winInputBox.FindName("imgIcon") as Image;

				// Show correct icon according to parameter
				switch (strIcon) {
					case "newUser":
						img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/Resources/newuser.png"));
						break;
					default:
						break;
				}

				// Set validation error message
				TextBlock txbValidation = winInputBox.FindName("txbValidation") as TextBlock;
				txbValidation.Text = strValidation;
				txbValidation.Visibility = Visibility.Visible;

				// Input text
				TextBox txt = winInputBox.FindName("txtInputText") as TextBox;

				// Pop-up message box
				winInputBox.ShowDialog();

				// Get return value
				if (winInputBox.strMsgReturn == null) {
					// Return cancel string
					strResult = "Cancel";
				} else if (winInputBox.strMsgReturn.Equals("")) {
					// Username was entered, return username
					strResult = txt.Text;
				} else {
					// Return cancel string
					strResult = winInputBox.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return strResult;
		}

		public static (string, string) ShowMessage(NewUser winNewUser, string strValidation) {
			string strResult = "";
			string strPicture = "";

			try {
				// Set validation error message
				TextBlock txbValidation = winNewUser.FindName("txbValidation") as TextBlock;
				txbValidation.Text = strValidation;
				txbValidation.Visibility = Visibility.Visible;

				// Input text
				TextBox txt = winNewUser.FindName("txtInputText") as TextBox;

				// Profile picture wrap panels
				WrapPanel wrpMales = winNewUser.FindName("wrpMales") as WrapPanel;
				WrapPanel wrpFemales = winNewUser.FindName("wrpFemales") as WrapPanel;

				// Pop-up message box
				winNewUser.ShowDialog();

				// Get return value
				if (winNewUser.strMsgReturn == null) {
					strResult = "Cancel";
				} else if (winNewUser.strMsgReturn.Equals("")) {
					// Username was entered
					// Set username and profile picture
					strResult = txt.Text;

					// Find which profile picture was selected
					if (wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					} else if (wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					}
				} else {
					strResult = winNewUser.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return (strResult, strPicture);
		}

		public static (string, string) ShowMessage(NewUser winNewUser) {
			string strResult = "";
			string strPicture = "";

			try {
				// Input text
				TextBox txt = winNewUser.FindName("txtInputText") as TextBox;

				// Profile picture wrap panels
				WrapPanel wrpMales = winNewUser.FindName("wrpMales") as WrapPanel;
				WrapPanel wrpFemales = winNewUser.FindName("wrpFemales") as WrapPanel;

				// Pop-up message box
				winNewUser.ShowDialog();

				// Get return value
				if (winNewUser.strMsgReturn == null) {
					strResult = "Cancel";
				} else if (winNewUser.strMsgReturn.Equals("")) {
					// Username was entered
					// Set username and profile picture
					strResult = txt.Text;

					// Find which profile picture was selected
					if (wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpMales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					} else if (wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value) != null) {
						strPicture = wrpFemales.Children.OfType<RadioButton>().FirstOrDefault(r => r.IsChecked.HasValue && r.IsChecked.Value).Name;
					}
				} else {
					strResult = winNewUser.strMsgReturn;
				}
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}

			return (strResult, strPicture);
		}

		public static string ShowMessage(PauseMenu winPause) {
			string strResult = "";

			// Pop-up message box and get return value
			winPause.ShowDialog();
			if (winPause.strMsgReturn == null) {
				strResult = "Resume";
			} else {
				strResult = winPause.strMsgReturn;
			}

			return strResult;
		}

		public static string ShowMessage(string strText, ConfirmMessageBox winConfirm) {
			string strResult = "";

			// Message text 
			TextBlock txb = winConfirm.FindName("txbMessageText") as TextBlock;
			txb.Text = strText;

			// Pop-up message box and get return value
			winConfirm.ShowDialog();
			if (winConfirm.strMsgReturn == null) {
				strResult = "Cancel";
			} else {
				strResult = winConfirm.strMsgReturn;
			}

			return strResult;
		}
	}
}
