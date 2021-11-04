using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using iSukces.DrawingPanel.Interfaces;
using TItem = iSukces.DrawingPanel.Interfaces.IDrawable;

namespace iSukces.DrawingPanel
{
    public class GroupDrawable : TItem, ISupportInitialize, IDisposable
    {
        public GroupDrawable()
        {
            _children                   =  new ExtendedObservableCollection<TItem>();
            _children.CollectionChanged += ChildrenOnCollectionChanged;
        }

        ~GroupDrawable() { DisposeInternal(); }

        public void BeginInit() { _suspendLevel++; }

        private void ChildrenOnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action != NotifyCollectionChangedAction.Move)
            {
                var l = e.OldItems;
                if (e.Action == NotifyCollectionChangedAction.Reset && e is ExtendedNotifyCollectionChangedEventArgs a1)
                {
                    l = a1.ResetedList;
                }

                if (l != null)
                    for (var i = 0; i < l.Count; i++)
                    {
                        var element = (TItem)l[i];
                        element.Changed -= ElementOnChanged;
                    }

                l = e.NewItems;
                if (l != null)
                    for (var i = 0; i < l.Count; i++)
                    {
                        var element = (TItem)l[i];
                        element.SetCanvasInfo(_canvasInfo);
                        element.Changed += ElementOnChanged;
                    }
            }

            OnChanged();
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
            _children = null;
        }


        public void Draw(Graphics graphics)
        {
            PendingDrawing = false;
            if (!Visible)
                return;
            for (var index = 0; index < _children.Count; index++)
            {
                var child = _children[index];
                child.Draw(graphics);
            }
        }

        private void ElementOnChanged(object sender, EventArgs e) { OnChanged(); }

        public void EndInit()
        {
            if (_suspendLevel < 1)
                throw new InvalidOperationException();
            _suspendLevel--;
            if (_suspendLevel == 0 && _needNotifyOnChanged)
            {
                _needNotifyOnChanged = false;
                OnChanged();
            }
        }

        private void OnChanged()
        {
            if (_suspendLevel > 0)
            {
                _needNotifyOnChanged = true;
                PendingDrawing       = true;
            }
            else
                _changed?.Invoke(this, EventArgs.Empty);
        }

        public void SetCanvasInfo(DrawingCanvasInfo canvasInfo)
        {
            _canvasInfo = canvasInfo;
            for (var index = 0; index < _children.Count; index++)
            {
                var i = _children[index];
                i.SetCanvasInfo(canvasInfo);
            }
        }

        public event EventHandler Changed
        {
            add
            {
#if DEBUG
                if (GetType().Name == "AutoCadLayersGroup")
                    Debug.Write("");
#endif
                _changed += value;
            }
            remove
            {
#if DEBUG
                if (GetType().Name == "AutoCadLayersGroup")
                    Debug.Write("");
#endif
                _changed -= value;
            }
        }

        public bool Visible
        {
            get => _visible;
            set
            {
                if (_visible == value)
                    return;
                _visible = value;
                OnChanged();
            }
        }

        public bool PresenterRenderingFlag { get; set; }
        public bool PendingDrawing         { get; private set; }

        public IList<TItem> Children => _children;

        public DrawingCanvasInfo CanvasInfo => _canvasInfo;

        public string Name { get; set; }
        public object Tag  { get; set; }
        private DrawingCanvasInfo _canvasInfo;

        private EventHandler _changed;
        private ExtendedObservableCollection<TItem> _children;
        private bool _needNotifyOnChanged;

        private int _suspendLevel;
        private bool _visible = true;
    }
}
