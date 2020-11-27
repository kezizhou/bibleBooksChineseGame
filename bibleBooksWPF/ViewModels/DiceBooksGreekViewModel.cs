using System;
using System.Collections.Generic;
using System.Windows;
using BibleBooksWPF.Helpers;
using BibleBooksWPF.Views;

namespace BibleBooksWPF.ViewModels {
	public class DiceBooksGreekViewModel : DiceBooksGameViewModel {
		public override List<string> lstRemaining { get; set; } = new List<string>(){
			"Matthew", "Mark", "Luke", "John", "Acts", "Romans", "1 Corinthians", "2 Corinthians", "Galatians", "Ephesians",
			"Philippians", "Colossians", "1 Thessalonians", "2 Thessalonians", "1 Timothy", "2 Timothy", "Titus",
			"Philemon", "Hebrews", "James", "1 Peter", "2 Peter", "1 John", "2 John", "3 John", "Jude", "Revelation"
		};

		public override List<string> lstChineseBooks { get; set; } = new List<string>() {
			"马太福音", "马可福音", "路加福音", "约翰福音", "使徒行传", "罗马书", "哥林多前书", "哥林多后书", "加拉太书", "以弗所书", "腓立比书", "歌罗西书", "帖撒罗尼迦前书",
			"帖撒罗尼迦后书", "提摩太前书", "提摩太后书", "提多书", "腓利门书", "希伯来书", "雅各书", "彼得前书", "彼得后书", "约翰一书", "约翰二书", "约翰三书", "犹大书", "启示录"
		};

		public override List<string> lstCompleted { get; set; } = new List<string>();

		public override void CompletedGame() {
			CustomMessageBox winMsgBox = new CustomMessageBox("Congratulations! You have finished. Try again?\n" +
								"Points Earned: " + propPoints + "\n" +
								"Time Elapsed: " + String.Format("{0:00}:{1:00}.{2:0000}", propTimeElapsed.Minutes, propTimeElapsed.Seconds, propTimeElapsed.Milliseconds), null);
			string strResponse = winMsgBox.strMsgReturn;

			switch (strResponse) {
				case "Retry":
					ChangeViewMessage.Navigate("DiceBooksGreek");
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
