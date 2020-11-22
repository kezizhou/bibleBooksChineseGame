﻿using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace BibleBooksWPF.Helpers {
    public class ObservableCollectionPropertyNotify<T> : ObservableCollection<T> {
        public void Refresh() {
            for (var i = 0; i < this.Count(); i++) {
                this.OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
            }
        }
    }
}
