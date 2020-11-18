using GalaSoft.MvvmLight.Messaging;

namespace BibleBooksWPF.Helpers {
	public class RefreshPageMessage {
		public string strPage { get; set; }

		public static void Refresh(string _strPage) {
			var refreshMessage = new RefreshPageMessage() { strPage = _strPage };
			Messenger.Default.Send<RefreshPageMessage>(refreshMessage);
		}
	}
}
