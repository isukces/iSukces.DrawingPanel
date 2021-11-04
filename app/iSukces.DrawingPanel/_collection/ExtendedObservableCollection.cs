using System;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    internal sealed class ExtendedObservableCollection<T> : ObservableCollection<T>,
        IExtendedObservableCollection<T>,
        IBla, ISupportInitialize
    {
        public void BeginInit()
        {
            _initializeLevel++;
            Flags |= BlaFlags.UnderInit;
        }


        protected override void ClearItems()
        {
            CheckReentrancy();
            if ((Flags & BlaFlags.UnderClear) != 0)
                throw new InvalidOperationException("Reentry to clear items");

            Flags |= BlaFlags.UnderClear;
            if (Items.Count > 0)
            {
                var args = new ExtendedNotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset);
                args.SetResetedList(Items);
                OnCollectionChanged(args);
            }

            base.ClearItems();
            Flags &= ~BlaFlags.UnderClear;
        }

        public void EndInit()
        {
            if (_initializeLevel < 1)
                throw new InvalidOperationException();
            _initializeLevel--;
            if (_initializeLevel == 0)
                Flags &= ~BlaFlags.UnderInit;
        }

        public BlaFlags Flags { get; private set; }

        
        private int _initializeLevel;
    }
}
