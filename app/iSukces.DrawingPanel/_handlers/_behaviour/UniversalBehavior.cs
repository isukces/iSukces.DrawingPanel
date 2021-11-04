#define KEYBOARD_FROM_FORM_
#define LOG
using System;
using System.Diagnostics;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public interface IBehaviorSource
    {
        Control MouseMoveFrom  { get; set; }
        Control MouseWheelFrom { get; set; }
        Control KeyboardFrom   { get; set; }
    }

    public sealed class UniversalBehavior : INewHandlerContainer, IBehaviorSource
    {
        public UniversalBehavior()
        {
            _handlers = new SortableSynchronizedCollection<HolderWrapper>();
        }

        [Conditional("DEBUG")]
        private static void Log(string text) { Debug.WriteLine("----------UniversalBehavior: " + text); }

        private bool Handle<THandler>(Func<THandler, DrawingHandleResult> func)
        {
            var handlersCopy = _handlers.GetSynchronizedArray();
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var index = 0; index < handlersCopy.Length; index++)
            {
                var handler = handlersCopy[index].Handler;
                if (!(handler is THandler)) continue;
                var myHandler = (THandler)handler;
                var result    = func(myHandler);
                if (result == DrawingHandleResult.Break)
                    return true;
            }

            return false;
        }

        private void HandleKeyDown(object sender, KeyEventArgs e)
        {
            e.Handled = Handle<INewKeyboardHandler>(h => h.HandleKeyDown(e));
            if (e.Handled)
                e.SuppressKeyPress = true;
        }

        private void HandleKeyUp(object sender, KeyEventArgs e)
        {
            e.Handled = Handle<INewKeyboardHandler>(h => h.HandleKeyUp(e));
        }

        private void HandleMouseWheel(object sender, MouseEventArgs e)
        {
            //DebugLog.Log("-------------- Wheel");
            // e.Handled = 
            Handle<INewMouseWheelHandler>(h => h.HandleMouseWheel(e));
        }

        private void HandleOnMouseDown(object sender, MouseEventArgs e)
        {
#if LOG
            Log("-------------- Down");
#endif
            Handle<INewMouseButtonHandler>(h =>
            {
                var result = h.HandleOnMouseDown(e);
                switch (result)
                {
                    case DrawingHandleResult.Break:
#if LOG
                        Log(nameof(HandleOnMouseDown) + " handled with " + h);
#endif
                        break;
                    case DrawingHandleResult.ContinueAfterAction:
#if LOG
                        Log(nameof(HandleOnMouseDown) + " partial handled with " + h);
#endif
                        break;
                }

                return result;
            });
        }

        private void HandleOnMouseMove(object sender, MouseEventArgs e)
        {
            if (this.KeyboardFrom != null)
            {
                if (!KeyboardFrom.Focused)
                    KeyboardFrom.Focus();
            }
            Handle<INewMouseButtonHandler>(h =>
            {
                // PdLog.DebugWriteLine("run UniversalBehavior.HandleOnMouseMove");
                return h.HandleOnMouseMove(e);
            });
        }

        private void HandleOnMouseUp(object sender, MouseEventArgs e)
        {
#if LOG
            Log("-------------- Up");
#endif
            Handle<INewMouseButtonHandler>(h =>
            {
                var result = h.HandleOnMouseUp(e);
                switch (result)
                {
                    case DrawingHandleResult.Break:
#if LOG
                        Log(nameof(HandleOnMouseUp) + " handled with " + h);
#endif
                        break;
                    case DrawingHandleResult.ContinueAfterAction:
#if LOG
                        Log(nameof(HandleOnMouseUp) + " partial with " + h);
#endif
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
                    _keyboardFrom.KeyUp -= HandleKeyUp;
                }
                _keyboardFrom = value;
                if (_keyboardFrom is not null)
                {
                    _keyboardFrom.KeyDown += HandleKeyDown;
                    _keyboardFrom.KeyUp   += HandleKeyUp;
                }
            }
        }

        /*protected override void OnAttached()
        {
            base.OnAttached();

            var eventControls = Controls;
            var mouse         = eventControls.MouseEventSource;
            mouse.MouseWheel += HandleMouseWheel;
            mouse.MouseDown  += HandleOnMouseDown;
            mouse.MouseMove  += HandleOnMouseMove;
            mouse.MouseUp    += HandleOnMouseUp;

#if KEYBOARD_FROM_FORM
            _mainWindow = Application.OpenForms.OfType<Form>().FirstOrDefault();
            if (_mainWindow != null)
            {
                _mainWindow.KeyDown += HandleKeyDown;
                _mainWindow.KeyUp += HandleKeyUp;
            }
            else
                throw new Exception("Unable to attach keyboard events");

            mouse.KeyDown += ScrollKeyDown;
#else
            var keyboard = eventControls.KeyboardEventSource;
            keyboard.KeyDown += HandleKeyDown;
            keyboard.KeyUp   += HandleKeyUp;
#endif
        }*/
 

        /*
        protected override void OnDetached()
        {
            var eventControls = Controls;
            var mouse         = eventControls.MouseEventSource;

            mouse.MouseWheel -= HandleMouseWheel;
            mouse.MouseDown  -= HandleOnMouseDown;
            mouse.MouseMove  -= HandleOnMouseMove;
            mouse.MouseUp    -= HandleOnMouseUp;

#if KEYBOARD_FROM_FORM
            if (_mainWindow != null)
            {
                _mainWindow.KeyDown -= HandleKeyDown;
                _mainWindow.KeyUp -= HandleKeyUp;

                _mainWindow = null;
            }
            scroll.KeyDown -= ScrollKeyDown;
#else
            var keyboard = eventControls.KeyboardEventSource;
            keyboard.KeyDown -= HandleKeyDown;
            keyboard.KeyUp   -= HandleKeyUp;
#endif
            base.OnDetached();
        }*/

        public void RegisterHandler(INewHandler handler, int order)
        {
            if (handler == null) return;
            lock(_handlers.SyncRoot)
            {
                UnregisterHandler(handler); // zabezpieczam się przed kopiami
                _handlers.Add(new HolderWrapper(handler, order));
                _handlers.Sort((a, b) => a.Order.CompareTo(b.Order));
            }
        }

        public void UnregisterHandler(INewHandler o)
        {
            if (o == null) return;
            lock(_handlers.SyncRoot)
            {
                for (var i = _handlers.Count - 1; i >= 0; i--)
                    if (_handlers[i].Handler.SameReference(o))
                        _handlers.RemoveAt(i);
            }
        }

        private readonly SortableSynchronizedCollection<HolderWrapper> _handlers;
        private Control _mouseMoveFrom;
        private Control _mouseWheelFrom;
        private Control _keyboardFrom;
#if KEYBOARD_FROM_FORM
        private Form _mainWindow;
#endif
    }
}
