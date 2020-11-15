
using GalaSoft.MvvmLight.Messaging;

namespace BibleBooksWPF.Helpers {
	public class ChangeViewMessage {
		public string strDestination { get; set; }

		public static void Navigate(string _strDestination) {
			var navMessage = new ChangeViewMessage() { strDestination = _strDestination };
			Messenger.Default.Send<ChangeViewMessage>(navMessage);
		}
	}
}
