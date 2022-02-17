using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.CompilerServices;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public static class DrawingLayersContainerHelper
    {
        public static void DrawablesCollectionChanged(NotifyCollectionChangedEventArgs e,
            bool isBackgroundLayer,
            DrawingCanvasInfo canvasInfo,
            EventHandler drawableChangedHandler,
            Action invalidateUnderlayBitmap)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void SayGoodByeToOldItems(IList oldItems)
            {
                if (oldItems == null) return;
                for (var index = oldItems.Count - 1; index >= 0; index--)
                    ((IDrawable)oldItems[index]).Changed -= drawableChangedHandler;
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

            if (isBackgroundLayer)
                invalidateUnderlayBitmap();

            SayGoodByeToOldItems(e.OldItems);
            var newItems = e.NewItems;
            if (newItems != null)
                for (var index = newItems.Count - 1; index >= 0; index--)
                {
                    var drawable = (IDrawable)newItems[index];
                    drawable.Changed += drawableChangedHandler;
                    drawable.SetCanvasInfo(canvasInfo);
                    drawable.PresenterRenderingFlag = isBackgroundLayer;
                }
        }
    }
}
