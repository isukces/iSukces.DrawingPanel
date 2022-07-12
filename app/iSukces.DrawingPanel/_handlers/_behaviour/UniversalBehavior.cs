#define KEYBOARD_FROM_FORM_
#define _LOG
using System;
using System.Diagnostics;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel
{
    public interface IBehaviorSource
    {
        #region properties

        Control MouseMoveFrom  { get; set; }
        Control MouseWheelFrom { get; set; }
        Control KeyboardFrom   { get; set; }

        #endregion
    }

    public sealed class UniversalBehavior : IDpHandlerContainer, IBehaviorSource
    {
        public UniversalBehavior()
        {
            _handlers = new SortableSynchronizedCollection<HolderWrapper>();
        }

        private bool Handle<THandler>(Func<THandler, DrawingHandleResult> func)
        {
            // ReSharper disable once InconsistentlySynchronizedField
            var handlersCopy = _handlers.GetSynchronizedArray();
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < handlersCopy.Length; index++)
            {
                var handler = handlersCopy[index].Handler;
                if (handler is not THandler myHandler) continue;
                var result    = func(myHandler);
                if (result == DrawingHandleResult.Break)
                    return true;
            }

            return false;
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = Handle<IDpKeyboardHandler>(h => h.HandleKeyDown(e));
            if (e.Handled)
                e.SuppressKeyPress = true;
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = Handle<IDpKeyboardHandler>(h => h.HandleKeyUp(e));
        }

        private void HandleMouseWheel(object sender, MouseEventArgs e)
        {
            Handle<IDpMouseWheelHandler>(h => h.HandleMouseWheel(e));
        }

        private void HandleOnMouseDown(object sender, MouseEventArgs e)
        {
            Handle<IDpMouseButtonHandler>(h =>
            {
                var result = h.HandleOnMouseDown(e);
                switch (result)
                {
                    case DrawingHandleResult.Break:
                        break;
                    case DrawingHandleResult.ContinueAfterAction:
                        break;
                }

                return result;
            });
        }

        private void HandleOnMouseMove(object sender, MouseEventArgs e)
        {
            if (KeyboardFrom != null)
            {
                if (!KeyboardFrom.Focused)
                    KeyboardFrom.Focus();
            }

            Handle<IDpMouseButtonHandler>(h =>
            {
                return h.HandleOnMouseMove(e);
            });
        }

        private void HandleOnMouseUp(object sender, MouseEventArgs e)
        {
            Handle<IDpMouseButtonHandler>(h =>
            {
                var result = h.HandleOnMouseUp(e);
                switch (result)
                {
                    case DrawingHandleResult.Break:
                        break;
                    case DrawingHandleResult.ContinueAfterAction:
                        break;
                }

                return result;
            });
        }


        public Control MouseMoveFrom
        {
            get => _mouseMoveFrom;
            set
            {
                if (ReferenceEquals(_mouseMoveFrom, value))
                    return;
                if (_mouseMoveFrom is not null)
                {
                    _mouseMoveFrom.MouseDown -= HandleOnMouseDown;
                    _mouseMoveFrom.MouseUp   -= HandleOnMouseUp;
                    _mouseMoveFrom.MouseMove -= HandleOnMouseMove;
                }

                _mouseMoveFrom = value;
                if (_mouseMoveFrom is not null)
                {
                    _mouseMoveFrom.MouseDown += HandleOnMouseDown;
                    _mouseMoveFrom.MouseUp   += HandleOnMouseUp;
                    _mouseMoveFrom.MouseMove += HandleOnMouseMove;
                }
            }
        }

        public Control MouseWheelFrom
        {
            get => _mouseWheelFrom;
            set
            {
                if (_mouseWheelFrom == value)
                    return;
                if (_mouseWheelFrom is not null)
                    _mouseWheelFrom.MouseWheel -= HandleMouseWheel;
                _mouseWheelFrom = value;
                if (_mouseWheelFrom is not null)
                    _mouseWheelFrom.MouseWheel += HandleMouseWheel;
            }
        }

        public Control KeyboardFrom
        {
            get => _keyboardFrom;
            set
            {
                if (_keyboardFrom == value)
                    return;
                if (_keyboardFrom is not null)
                {
                    _keyboardFrom.KeyDown -= HandleKeyDown;
                    _keyboardFrom.KeyUp   -= HandleKeyUp;
                }

                _keyboardFrom = value;
                if (_keyboardFrom is not null)
                {
                    _keyboardFrom.KeyDown += HandleKeyDown;
                    _keyboardFrom.KeyUp   += HandleKeyUp;
                }
            }
        }


        public void RegisterHandler(IDpHandler handler, int order)
        {
            if (handler == null) return;
            lock(_handlers.SyncRoot)
            {
                UnregisterHandler(handler); // zabezpieczam się przed kopiami
                _handlers.Add(new HolderWrapper(handler, order));
                _handlers.Sort((a, b) => a.Order.CompareTo(b.Order));
            }
        }

        public void UnregisterHandler(IDpHandler o)
        {
            if (o == null) return;
            lock(_handlers.SyncRoot)
            {
                for (var i = _handlers.Count - 1; i >= 0; i--)
                    if (_handlers[i].Handler.SameReference(o))
                        _handlers.RemoveAt(i);
            }
        }

        [NotNull]
        private readonly SortableSynchronizedCollection<HolderWrapper> _handlers;
        private Control _mouseMoveFrom;
        private Control _mouseWheelFrom;
        private Control _keyboardFrom;
#if KEYBOARD_FROM_FORM
        private Form _mainWindow;
#endif
    }
}
