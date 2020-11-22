using GalaSoft.MvvmLight.Messaging;
using System;

namespace BibleBooksWPF.Helpers {
	public class NotificationMessageAction<T, TCallbackParameter> : NotificationMessageAction<TCallbackParameter> {
		public T Content { get; }

		public NotificationMessageAction(T _content, string strNotification, Action<TCallbackParameter> callback) :
			base(strNotification, callback) {
			Content = _content;
		}
	}
}
