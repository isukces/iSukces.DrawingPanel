#nullable disable
using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

internal sealed class ExtendedObservableCollection<T> : ObservableCollection<T>,
    IExtendedObservableCollection<T>, ISupportInitialize
{
    public void BeginInit()
    {
        _initializeLevel++;
        _flags |= InitializationFlags.UnderInit;
    }


    protected override void ClearItems()
    {
        CheckReentrancy();
        if ((_flags & InitializationFlags.UnderClear) != 0)
            throw new InvalidOperationException("Reentry to clear items");

        _flags |= InitializationFlags.UnderClear;
        if (Items.Count > 0)
        {
            var args = new ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
            args.SetResetedList(Items);
            OnCollectionChanged(args);
        }

        base.ClearItems();
        _flags &= ~InitializationFlags.UnderClear;
    }

    public void EndInit()
    {
        if (_initializeLevel < 1)
            throw new InvalidOperationException();
        _initializeLevel--;
        if (_initializeLevel == 0)
            _flags &= ~InitializationFlags.UnderInit;
    }

    #region Fields

    private int _initializeLevel;
    private InitializationFlags _flags;

    #endregion


    [Flags]
    public enum InitializationFlags
    {
        None = 0,
        UnderClear = 1,
        UnderInit = 2
    }
}
