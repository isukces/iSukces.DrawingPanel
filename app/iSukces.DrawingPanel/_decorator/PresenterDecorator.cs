using System;
using System.ComponentModel;
using System.Drawing;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public abstract class PresenterDecorator<TPresenter> : IDrawable, ISupportInitialize, IDisposable
    {
        protected PresenterDecorator(TPresenter presenter)
        {
            Presenter = presenter;
        }

        ~PresenterDecorator()
        {
            DisposeInternal(false);
        }

        public void BeginInit()
        {
            _suspendLevel++;
        }

        public void Dispose()
        {
            DisposeInternal(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void DisposeInternal(bool disposing)
        {
            if (disposing)
            {
                // Release managed resources here
            }
            // Release unmanaged resources here
        }

        public abstract void Draw(Graphics graphics);

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

        protected void OnChanged()
        {
            if (_suspendLevel > 0)
                _needNotifyOnChanged = true;
            else
                Changed?.Invoke(this, EventArgs.Empty);
        }

        public virtual void SetCanvasInfo(DrawingCanvasInfo canvasInfo)
        {
            CanvasInfo = canvasInfo;
        }

        public event EventHandler Changed;

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

        public    bool              PresenterRenderingFlag { get; set; }
        protected DrawingCanvasInfo CanvasInfo             { get; private set; }
        protected TPresenter        Presenter              { get; }

        private bool _needNotifyOnChanged;
        private int _suspendLevel;
        private bool _visible = true;
    }
}
