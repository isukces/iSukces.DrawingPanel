using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using iSukces.DrawingPanel.Interfaces;
using TItem = iSukces.DrawingPanel.Interfaces.IDrawableWithLayer;

namespace iSukces.DrawingPanel
{
    /// <summary>
    ///     Allows to add/remove a bunch of drawables
    /// </summary>
    public sealed class DrawingContainerAdapter : ISupportInitialize, IDisposable, IDrawingCanvasInfoProvider
    {
        public DrawingContainerAdapter(IDrawingLayersContainer container)
        {
            _container                  =  container;
            _children                   =  new ExtendedObservableCollection<TItem>();
            _children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        ~DrawingContainerAdapter() { DisposeInternal(); }

        public void BeginInit()
        {
            if (_container is ISupportInitialize supportInitialize)
                supportInitialize.BeginInit();
            _children?.BeginInit();
        }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void SayGoodByeToOldItems(IList oldItems)
            {
                if (oldItems == null) return;
                for (var i = oldItems.Count - 1; i >= 0; i--)
                {
                    var element    = (TItem)oldItems[i];
                    var collection = _container.Get(element.DrawableLayer);
                    collection.Remove(element);
                }
            }

            if (e is ExtendedNotifyCollectionChangedEventArgs ee)
            {
                if (ee.Action != NotifyCollectionChangedAction.Reset)
                    throw new InvalidOperationException();
                SayGoodByeToOldItems(ee.ResetedList);
                return;
            }

            if (e.Action is NotifyCollectionChangedAction.Move or NotifyCollectionChangedAction.Reset)
                return;

            SayGoodByeToOldItems(e.OldItems);
            var l = e.NewItems;
            if (l != null)
                for (var i = 0; i < l.Count; i++)
                {
                    var element    = (TItem)l[i];
                    var collection = _container.Get(element.DrawableLayer);
                    collection.Add(element);
                }
        }

        public void Dispose()
        {
            DisposeInternal();
            GC.SuppressFinalize(this);
        }

        private void DisposeInternal()
        {
            if (_children is null)
                return;
            _children.Clear();
            _children  = null;
            _container = null;
        }

        public void EndInit()
        {
            _children?.EndInit();
            if (_container is ISupportInitialize supportInitialize)
                supportInitialize.EndInit();
        }

        public DrawingCanvasInfo CanvasInfo => _container.CanvasInfo;

        public IList<TItem> Children => _children;


        public string Name { get; set; }
        public object Tag  { get; set; }
        private ExtendedObservableCollection<TItem> _children;

        private IDrawingLayersContainer _container;
    }
}
