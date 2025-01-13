#nullable disable
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel;

public static class CollectionFactory
{
    /// <summary>
    /// Creates instance of IExtendedObservableCollection 
    /// </summary>
    /// <param name="handler"></param>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static IExtendedObservableCollection<T> Make<T>(NotifyCollectionChangedEventHandler handler)
    {
        var result = new ExtendedObservableCollection<T>();
        if (handler != null)
            result.CollectionChanged += handler;
        return result;
    }
}
