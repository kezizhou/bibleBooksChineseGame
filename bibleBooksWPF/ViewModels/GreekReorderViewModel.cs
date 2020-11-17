using System.Collections.Generic;

namespace BibleBooksWPF.ViewModels {
	public class GreekReorderViewModel : ReorderGameViewModel {

		public GreekReorderViewModel() {
			propBooksRemaining = new List<string>(propGreek);
		}

		private string[] astrGreek = { "lblMatthew", "lblMark", "lblLuke", "lblJohn", "lblActs", "lblRomans", "lbl1Corinthians", "lbl2Corinthians", "lblGalatians", "lblEphesians",
									"lblPhilippians", "lblColossians", "lbl1Thessalonians", "lbl2Thessalonians", "lbl1Timothy", "lbl2Timothy", "lblTitus",
									"lblPhilemon", "lblHebrews", "lblJames", "lbl1Peter", "lbl2Peter", "lbl1John", "lbl2John", "lbl3John", "lblJude", "lblRevelation" };
		public string[] propGreek {
			get { 
				return astrGreek;
			}
			set { 
				astrGreek = value;
				NotifyPropertyChanged();
			}
		}

		private string[] astrReorderLbls = { "lbl1", "lbl2", "lbl3", "lbl4", "lbl5", "lbl6", "lbl7", "lbl8", "lbl9", "lbl10", "lbl11", "lbl12", "lbl13", "lbl14",
										 "lbl15", "lbl16", "lbl17", "lbl18", "lbl19", "lbl20", "lbl21", "lbl22", "lbl23", "lbl24", "lbl25", "lbl26", "lbl27"};
		public string[] propReorderLbls {
			get { 
				return astrReorderLbls; 
			}
			set { 
				astrReorderLbls = value;
				NotifyPropertyChanged();
			}
		}

		private string[] astrChinese = { "马太福音", "马可福音", "路加福音", "约翰福音", "使徒行传", "罗马书", "哥林多前书", "哥林多后书", "加拉太书", "以弗所书", "腓立比书", "歌罗西书", "帖撒罗尼迦前书",
										"帖撒罗尼迦后书", "提摩太前书", "提摩太后书", "提多书", "腓利门书", "希伯来书", "雅各书", "彼得前书", "彼得后书", "约翰一书", "约翰二书", "约翰三书", "犹大书", "启示录"};
		public string[] propChinese {
			get { 
				return astrChinese; 
			}
			set { 
				astrChinese = value;
				NotifyPropertyChanged();
			}
		}
	}
}
