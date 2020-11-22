using System.Windows;

namespace BibleBooksWPF.Models {
	public class DiceBooksBibleBook {
		public string propBook { get; set; }
		public Visibility propVisibility { get; set; } = Visibility.Hidden;

		public DiceBooksBibleBook() {

		}

		public DiceBooksBibleBook(string propBook) {
			this.propBook = propBook;
		}
	}
}
