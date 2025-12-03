using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace iSukces.DrawingPanel;

public sealed class ExtendedNotifyCollectionChangedEventArgs : NotifyCollectionChangedEventArgs
{
    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action)
        : base(action)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem)
        : base(action, changedItem)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem,
        int index)
        : base(action, changedItem, index)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems)
        : base(action, changedItems)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems,
        int startingIndex)
        : base(action, changedItems, startingIndex)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem,
        object oldItem)
        : base(action, newItem, oldItem)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object newItem,
        object oldItem, int index)
        : base(action, newItem, oldItem, index)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems,
        IList oldItems)
        : base(action, newItems, oldItems)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList newItems,
        IList oldItems, int startingIndex)
        : base(action, newItems, oldItems, startingIndex)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, object changedItem,
        int index, int oldIndex)
        : base(action, changedItem, index, oldIndex)
    {
    }

    public ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction action, IList changedItems,
        int index, int oldIndex)
        : base(action, changedItems, index, oldIndex)
    {
    }

    public void SetResetedList<T>(IList<T> items)
    {
        if (items is IList list)
        {
            ResetedList = list;
            return;
        }

        ResetedList = items.ToArray();
    }

    public IList ResetedList { get; private set; }
}
