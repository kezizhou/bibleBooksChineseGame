using BibleBooksWPF.Views;
using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;

namespace BibleBooksWPF.Models {
	public class BaseStatistic {
		public string strLabel { get; set; }
		public string strGame { get; set; }
		public int intRecordPoint { get; set; }
		public TimeSpan tsRecordTime { get; set; }
		public double dblAveragePoint { get; set; }
		public TimeSpan tsAverageTime { get; set; }

		public BaseStatistic() {

		}

		public BaseStatistic(string strLabel, string strGame, int intRecordPoint, TimeSpan tsRecordTime, double dblAveragePoint, TimeSpan tsAverageTime) {
			this.strLabel = strLabel;
			this.strGame = strGame;
			this.intRecordPoint = intRecordPoint;
			this.tsRecordTime = tsRecordTime;
			this.dblAveragePoint = dblAveragePoint;
			this.tsAverageTime = tsAverageTime;
		}

		public static BaseStatistic GetBaseStatistic(string strLabel, string strGame) {
			if (App.Current.Properties["currentUsername"] == null) return null;

			BaseStatistic baseStatistic;
			int intRecordPoint = 0;
			TimeSpan tsRecordTime = new TimeSpan();
			double dblAveragePoint = 0;
			TimeSpan tsAverageTime = new TimeSpan();

			JObject obj = JObject.Parse(File.ReadAllText(Globals.usersFilePath));
			JToken userGameToken = obj.SelectToken($"$.Users[?(@.username == '{App.Current.Properties["currentUsername"]}')].lstGameStatistics.GameStatistics[?(@.strName == '{strGame}')]");
			GameStatistics gsTotalGames = userGameToken.ToObject<GameStatistics>();

			if (gsTotalGames.lintPoints.Count != 0 && gsTotalGames.ltsTimeElapsed.Count != 0) {
				intRecordPoint = gsTotalGames.lintPoints.Max();
				tsRecordTime = gsTotalGames.ltsTimeElapsed.Min();
				dblAveragePoint = gsTotalGames.lintPoints.Average();
				TimeSpan tsAverageUntrimmed = TimeSpan.FromSeconds(gsTotalGames.ltsTimeElapsed.Average(timeSpan => timeSpan.TotalSeconds));
				tsAverageTime = new TimeSpan(tsAverageUntrimmed.Hours, tsAverageUntrimmed.Minutes, tsAverageUntrimmed.Seconds);
			}

			baseStatistic = new BaseStatistic(strLabel, strGame, intRecordPoint, tsRecordTime, dblAveragePoint, tsAverageTime);

			return baseStatistic;
		}
	}
}
