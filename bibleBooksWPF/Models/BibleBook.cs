
namespace BibleBooksWPF.Models {
	public class BibleBook {
		public string strEnglish { get; set; }
		public string strChinese { get; set; }
		public double dblTranslateTransformX { get; set; } = 0;
		public double dblTranslateTransformY { get; set; } = 0;

		public BibleBook(string strEnglish, string strChinese) {
			this.strEnglish = strEnglish;
			this.strChinese = strChinese;
		}

		public BibleBook() {

		}
	}
}
