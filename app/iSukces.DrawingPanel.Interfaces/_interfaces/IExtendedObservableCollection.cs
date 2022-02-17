using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;

namespace iSukces.DrawingPanel.Interfaces
{
    /// <summary>
    ///     Mostly the same as ObservableCollection but Clear methods removes all item by item
    ///     in Clear method. This allows to handle series of NotifyCollectionChangedAction.Remove
    ///     before NotifyCollectionChangedAction.Reset
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IExtendedObservableCollection<T> : IList<T>,
        INotifyCollectionChanged,
        INotifyPropertyChanged
    {
    }

}
