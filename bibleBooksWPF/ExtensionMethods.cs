using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace ExtensionMethods {
    public static class LabelExt {
		public static void BringToFront(this Label lbl) {
			if (lbl == null) return;

			Grid parent = lbl.Parent as Grid;
			if (parent == null) return;

			var maxZ = parent.Children.OfType<UIElement>()
			  .Where(x => x != lbl)
			  .Select(x => Grid.GetZIndex(x))
			  .Max();
			Grid.SetZIndex(lbl, maxZ + 1);
		}
	}

	public static class CustomMessageBoxMethods {
		public static string ShowMessage(string strMessage, string strTitle, string strIcon, BibleBooksWPF.CustomMessageBox winMsgBox) {
			// Message text 
			TextBlock txb = winMsgBox.FindName("txbMessageText") as TextBlock;
			txb.Text = strMessage;

			// Title
			winMsgBox.Title = strTitle;

			// Icon
			Image img = winMsgBox.FindName("imgIcon") as Image;

			// Show correct icon according to parameter
			switch (strIcon) {
				// Icon made by Flat Icons from www.flaticon.com
				case "congrats":
					img.Source = new BitmapImage(new Uri("pack://application:,,,/BibleBooksWPF;component/congrats.png"));
					break;
				default:
					break;
			}

			// Pop-up message box and get return value
			string strResult = winMsgBox.strMsgReturn;
			return strResult;
		}
	}
}
