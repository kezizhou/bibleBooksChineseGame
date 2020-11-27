using System;
using System.Collections.Generic;
using System.Windows;
using BibleBooksWPF.Helpers;
using BibleBooksWPF.Views;

namespace BibleBooksWPF.ViewModels {
	public class DiceBooksHebrewViewModel : DiceBooksGameViewModel {
		public override List<string> lstRemaining { get; set; } = new List<string>(){
			"Genesis", "Exodus", "Leviticus", "Numbers", "Deuteronomy", "Joshua", "Judges", "Ruth", "1 Samuel",
			"2 Samuel", "1 Kings", "2 Kings", "1 Chronicles", "2 Chronicles", "Ezra", "Nehemiah", "Esther", "Job",
			"Psalms", "Proverbs", "Ecclesiastes", "Song of Solomon", "Isaiah", "Jeremiah", "Lamentations", "Ezekiel",
			"Daniel", "Hosea", "Joel", "Amos", "Obadiah", "Jonah", "Micah", "Nahum", "Habakkuk", "Zephaniah",
			"Haggai", "Zechariah", "Malachi"
		};

		public override List<string> lstChineseBooks { get; set; } = new List<string>() {
			"创世记", "出埃及记", "利未记", "民数记", "申命记", "约书亚记", "士师记", "路得记", "撒母耳记上",
			"撒母耳记下", "列王纪上", "列王纪下", "历代志上", "历代志下", "以斯拉记", "尼希米记", "以斯帖记", "约伯记",
			"诗篇", "箴言", "传道书", "雅歌", "以赛亚书", "耶利米书", "耶利米哀歌", "以西结书",
			"但以理书", "何西阿书", "约珥书", "阿摩司书", "俄巴底亚书", "约拿书", "弥迦书", "那鸿书", "哈巴谷书", "西番雅书",
			"哈该书", "撒迦利亚书", "玛拉基书"
		};

		public override List<string> lstCompleted { get; set; } = new List<string>();

		public override void CompletedGame() {
			CustomMessageBox winMsgBox = new CustomMessageBox("Congratulations! You have finished. Try again?\n" +
								"Points Earned: " + propPoints + "\n" +
								"Time Elapsed: " + String.Format("{0:00}:{1:00}.{2:0000}", propTimeElapsed.Minutes, propTimeElapsed.Seconds, propTimeElapsed.Milliseconds), null);
			string strResponse = winMsgBox.strMsgReturn;

			switch (strResponse) {
				case "Retry":
					ChangeViewMessage.Navigate("DiceBooksHebrew");
					break;
				case "Main":
					ChangeViewMessage.Navigate("MainMenu");
					break;
				case "Exit":
					Application.Current.Shutdown();
					break;
				default:
					break;
			}
		}
	}
}
