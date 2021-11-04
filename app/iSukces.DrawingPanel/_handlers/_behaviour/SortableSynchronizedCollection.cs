using System;
using System.Collections.Generic;

namespace iSukces.DrawingPanel
{
    public sealed class SortableSynchronizedCollection<T> : SynchronizedCollection<T>
    {
        public T[] GetSynchronizedArray()
        {
            lock(SyncRoot)
            {
                var result = new T[Count];
                CopyTo(result, 0);
                return result;
            }
        }

        public void Sort(IComparer<T> comparer)
        {
            lock(SyncRoot)
            {
                Items.Sort(comparer);
            }
        }

        public void Sort(Comparison<T> comparison)
        {
            lock(SyncRoot)
            {
                Items.Sort(comparison);
            }
        }
    }
}