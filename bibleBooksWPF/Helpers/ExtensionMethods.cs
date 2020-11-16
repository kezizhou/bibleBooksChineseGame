using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;
using System.Windows.Media;
using BibleBooksWPF.UserControls;

namespace BibleBooksWPF.Helpers {
    public static class LabelExt {
		public static void BringToFront(this Label lbl) {
			try {
				if (lbl == null) return;

				Grid parent = lbl.Parent as Grid;
				if (parent == null) return;

				var maxZ = parent.Children.OfType<UIElement>()
				  .Where(x => x != lbl)
				  .Select(x => Grid.GetZIndex(x))
				  .Max();
				Grid.SetZIndex(lbl, maxZ + 1);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
		public static void BringToFront(this BibleBook lbl) {
			try {
				if (lbl == null) return;

				Grid parent = lbl.Parent as Grid;
				if (parent == null) return;

				var maxZ = parent.Children.OfType<UIElement>()
				  .Where(x => x != lbl)
				  .Select(x => Grid.GetZIndex(x))
				  .Max();
				Grid.SetZIndex(lbl, maxZ + 1);
			} catch (Exception ex) {
				MessageBox.Show(ex.Message);
			}
		}
	}

	public static class Layout {
		public static int TransformToPixels(Visual visual, double dblXY) {
			Matrix matrix;
			var source = PresentationSource.FromVisual(visual);

			if (source != null) {
				matrix = source.CompositionTarget.TransformToDevice;
			} else {
				using (var src = new HwndSource(new HwndSourceParameters())) {
					matrix = src.CompositionTarget.TransformToDevice;
				}
			}

			int intPixelXY = (int)(matrix.M11 * dblXY);

			return intPixelXY;
		}
	}
}
