using System.Collections.Specialized;
using System.Runtime.CompilerServices;

namespace iSukces.DrawingPanel
{
    internal static class CollectionFactory
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ExtendedObservableCollection<T> Make<T>(NotifyCollectionChangedEventHandler handler)
        {
            var result = new ExtendedObservableCollection<T>();
            if (handler != null)
                result.CollectionChanged += handler;
            return result;
        }

        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static ObservableCollectionEx<T> Make<T>()
        {
            return new ObservableCollectionEx<T>();
        }*/

        /*[MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ObservableCollectionEx<T> Make<T>(params T[] elements)
        {
            return new ObservableCollectionEx<T>(elements);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ObservableCollectionEx<T> Make<T>(IEnumerable<T> elements)
        {
            return new ObservableCollectionEx<T>(elements);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ObservableCollectionEx<T> Make<T>(List<T> elements)
        {
            return new ObservableCollectionEx<T>(elements);
        }*/
    }
}
