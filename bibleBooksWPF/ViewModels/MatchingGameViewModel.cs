
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace BibleBooksWPF.Classes {
	class MatchingGameViewModel : INotifyPropertyChanged {

		private int intNumberAttempted = 0;
		public int propNumberAttempted {
			get { 
				return intNumberAttempted;
			}
			set { 
				intNumberAttempted = value;
				NotifyPropertyChanged();
			}
		}

		private int intNumberCorrect = 0;
		public int propNumberCorrect {
			get { 
				return intNumberCorrect;
			}
			set { 
				intNumberCorrect = value;
				NotifyPropertyChanged();
			}
		}

		private int intCurrentPoints = 0;
		public int propCurrentPoints {
			get { 
				return intCurrentPoints;
			}
			set { 
				intCurrentPoints = value;
				NotifyPropertyChanged();
			}
		}

		private long lngTotalPoints = Properties.Settings.Default.lngTotalPoints;
		public long propTotalPoints {
			get { 
				return lngTotalPoints; 
			}
			set {
				if (value == lngTotalPoints) return;
				lngTotalPoints = value;
				Properties.Settings.Default.lngTotalPoints = value;
				Properties.Settings.Default.Save();
				NotifyPropertyChanged();
			}
		}

		private TimeSpan tsTimeElapsed = new TimeSpan(0, 0, 0);
		public TimeSpan propTimeElapsed {
			get { 
				return tsTimeElapsed; 
			}
			set { 
				tsTimeElapsed = value;
				NotifyPropertyChanged();
			}
		}

		internal void AddCorrectAttempt() {
			propNumberAttempted += 1;
			propNumberCorrect += 1;
			propCurrentPoints += 1;
			propTotalPoints += 1;	
		}

		internal void AddIncorrectAttempt() {
			propCurrentPoints -= 1;
			propNumberAttempted += 1;
			Properties.Settings.Default.lngTotalPoints -= 1;
			Properties.Settings.Default.Save();
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void NotifyPropertyChanged([CallerMemberName] string strPropertyName = null) {
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(strPropertyName));
		}
	}
}
