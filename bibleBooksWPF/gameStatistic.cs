using System;
using System.Collections.Generic;
using System.Linq;

namespace BibleBooksWPF {
	class GameStatistics {
		public string strName { get; set; }
		public List<int> lintPoints { get; set; }
		public List<TimeSpan> ltsTimeElapsed { get; set; }

		public GameStatistics(string strName, List<int> lintPoints, List<TimeSpan> ltsTimeElapsed) {
			this.strName = strName;
			this.lintPoints = lintPoints;
			this.ltsTimeElapsed = ltsTimeElapsed;
		}

		public GameStatistics(string strName) {
			this.strName = strName;
			this.lintPoints = new List<int>();
			this.ltsTimeElapsed = new List<TimeSpan>();
		}

		public GameStatistics() {
			this.lintPoints = new List<int>();
			this.ltsTimeElapsed = new List<TimeSpan>();
		}
	}

	class RootGameStatistics {
		public List<GameStatistics> GameStatistics { get; set; }

		public RootGameStatistics(IEnumerable<GameStatistics> GameStatistics) {
			this.GameStatistics = GameStatistics.ToList();
		}
	}
}
