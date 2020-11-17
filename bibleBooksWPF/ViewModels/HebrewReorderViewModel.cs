using System;
using System.Collections.Generic;

namespace BibleBooksWPF.ViewModels {
	public class HebrewReorderViewModel : ReorderGameViewModel {

		public HebrewReorderViewModel() {
			propBooksRemaining = new List<string>(propHebrew);
		}

		private string[] astrHebrew = { "lblGenesis", "lblExodus", "lblLeviticus", "lblNumbers", "lblDeuteronomy", "lblJoshua", "lblJudges", "lblRuth", "lbl1Samuel",
										"lbl2Samuel", "lbl1Kings", "lbl2Kings", "lbl1Chronicles", "lbl2Chronicles", "lblEzra", "lblNehemiah", "lblEsther", "lblJob",
										"lblPsalms", "lblProverbs", "lblEcclesiastes", "lblSongofSolomon", "lblIsaiah", "lblJeremiah", "lblLamentations", "lblEzekiel",
										"lblDaniel", "lblHosea", "lblJoel", "lblAmos", "lblObadiah", "lblJonah", "lblMicah", "lblNahum", "lblHabakkuk", "lblZephaniah",
										"lblHaggai", "lblZechariah", "lblMalachi"};
		public string[] propHebrew {
			get {
				return astrHebrew;
			}
			set {
				astrHebrew = value;
				NotifyPropertyChanged();
			}
		}

		private string[] astrReorderLbls = { "lbl1", "lbl2", "lbl3", "lbl4", "lbl5", "lbl6", "lbl7", "lbl8", "lbl9", "lbl10", "lbl11", "lbl12", "lbl13", "lbl14",
										 "lbl15", "lbl16", "lbl17", "lbl18", "lbl19", "lbl20", "lbl21", "lbl22", "lbl23", "lbl24", "lbl25", "lbl26", "lbl27",
										 "lbl28", "lbl29", "lbl30", "lbl31", "lbl32", "lbl33", "lbl34", "lbl35", "lbl36", "lbl37", "lbl38", "lbl39"};
		public string[] propReorderLbls {
			get {
				return astrReorderLbls;
			}
			set {
				astrReorderLbls = value;
				NotifyPropertyChanged();
			}
		}

		private string[] astrChinese = { "创世记", "出埃及记", "利未记", "民数记", "申命记", "约书亚记", "士师记", "路得记", "撒母耳记上", "撒母耳记下", "列王纪上", "列王纪下",
										"历代志上", "历代志下", "以斯拉记", "尼希米记", "以斯帖记", "约伯记", "诗篇", "箴言", "传道书", "雅歌", "以赛亚书", "耶利米书", "耶利米哀歌",
										"以西结书", "但以理书", "何西阿书", "约珥书", "阿摩司书", "俄巴底亚书", "约拿书", "弥迦书", "那鸿书", "哈巴谷书", "西番雅书", "哈该书",
										"撒迦利亚书", "玛拉基书"};
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
