using System.Windows;

namespace BibleBooksWPF.Models {
	public class DiceBooksEntry {
		public string strInput { get; set; } = "";
		public Visibility visible { get; set; } = Visibility.Visible;
		public bool? blnCorrect { get; set; } = null;
		public bool blnSubmitted { get; set; } = false;

		public DiceBooksEntry() {

		}
	}
}
