#if DEBUG
#define __USE_Debug_WriteLine
#endif
using System;
using System.Collections;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;

namespace iSukces.DrawingPanel
{
    public partial class DrawingLayersContainer : Control, ICadControlLogicOwner
    {
        public DrawingLayersContainer()
        {
            SetUnderLayerOpacity(1);
            ColorScheme = new DrawingColorScheme();
            ((DrawingColorScheme)ColorScheme).Update(BackColor);

            _logic                              = new CadControlLogic(this);
            _logic.BehaviorSource.MouseMoveFrom = this;

            Overlay   = CollectionFactory.Make<IDrawable>(DrawablesCollectionChanged);
            Drawables = CollectionFactory.Make<IDrawable>(DrawablesCollectionChanged);
            Underlay  = CollectionFactory.Make<IDrawable>(DrawablesCollectionChanged);
            _layers   = new[] { Overlay, Drawables, Underlay };

            SetStyle(ControlStyles.OptimizedDoubleBuffer, true);
        }

        private static void DrawBitmapWithAttributes(Graphics graphics, Image bmp, ImageAttributes imageAtt)
        {
            var iWidth  = bmp.Width;
            var iHeight = bmp.Height;

            graphics.DrawImage(
                bmp,
                new Rectangle(0, 0, iWidth, iHeight), // Destination rectangle
                0, // Source rectangle X 
                0, // Source rectangle Y
                iWidth, // Source rectangle width
                iHeight, // Source rectangle height
                GraphicsUnit.Pixel,
                imageAtt);

            //   bmp.Save("e:\\temp\\aaaa.png");
        }

        public void ApplyBounds(Rect bounds)
        {
            _logic.ApplyBounds(bounds);
        }

        public void ClearAll()
        {
            BeginInit();
            Overlay.Clear();
            Drawables.Clear();
            Underlay.Clear();
            EndInit();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
                _logic.Dispose();
            base.Dispose(disposing);
        }

        private void DrawablesCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            void SayGoodByeToOldItems(IList oldItems)
            {
                if (oldItems == null) return;
                for (var index = oldItems.Count - 1; index >= 0; index--)
                    ((IDrawable)oldItems[index]).Changed -= DrawableChanged;
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

            var isBackgroundLayer = ReferenceEquals(sender, Underlay);
            if (isBackgroundLayer)
                InvalidateBitmap();

            void DrawableChanged(object changedDrawable, EventArgs ignore2)
            {
                var d = (IDrawable)changedDrawable;
                if (d.PresenterRenderingFlag)
                    InvalidateBitmap();
                InvalidateRequest();
            }

            SayGoodByeToOldItems(e.OldItems);
            var newItems = e.NewItems;
            if (newItems != null)
                for (var index = newItems.Count - 1; index >= 0; index--)
                {
                    var drawable = (IDrawable)newItems[index];
                    drawable.Changed += DrawableChanged;
                    drawable.SetCanvasInfo(_logic.CanvasInfo);
                    drawable.PresenterRenderingFlag = isBackgroundLayer;
                }

#if DEBUG
            if (_suspendLevel > 0)
            {
                _needInvalidate = true;
                return;
            }
#endif
            InvalidateRequest();
        }


        EventControls ICadControlLogicOwner.GetEventSourceControl()
        {
            return new(this, this, this);
        }

        private void InvalidateBitmap()
        {
            if (_backgroundBitmap is null)
                return;
            _backgroundBitmap.Dispose();
            _backgroundBitmap = null;
        }

        protected override void OnBackColorChanged(EventArgs e)
        {
            base.OnBackColorChanged(e);
            ((DrawingColorScheme)ColorScheme).Update(BackColor);
        }

        protected override void OnClientSizeChanged(EventArgs e)
        {
            base.OnClientSizeChanged(e);
            _logic.SetDrawingSize();
            InvalidateRequest();
        }

        protected virtual void OnDrawingTranformChanged()
        {
            DrawingTranformChanged?.Invoke(this, EventArgs.Empty);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
#if USE_Debug_WriteLine
            Debug.WriteLine("DrawingLayersContainer.OnPaint");
#endif
            base.OnPaint(e);
            // Stopwatch w = new Stopwatch(); w.Start();
            if (_suspendLevel > 0)
            {
                _needInvalidate = true;
                return;
            }

            var needDrawBg = true;

            for (var layerIdx = _layers.Length - 1; layerIdx >= 0; layerIdx--)
            {
                var drawables      = _layers[layerIdx];
                var drawablesCount = drawables.Count;

                var isBackgroundLayer = cache && layerIdx == 2;
                var graphics          = e.Graphics;
                var disposeGraphics   = false;
                if (isBackgroundLayer)
                {
                    if (drawablesCount == 0)
                    {
                        InvalidateBitmap();
                        continue;
                    }

                    if (_backgroundBitmap is null)
                    {
                        var clientSize = ClientSize;
                        _backgroundBitmap = new Bitmap(clientSize.Width, clientSize.Height);
                        graphics          = Graphics.FromImage(_backgroundBitmap);
                        graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
                        disposeGraphics = true;
                    }
                }
                else
                {
                    if (needDrawBg)
                    {
                        graphics.FillRectangle(new SolidBrush(BackColor), ClientRectangle);
                        needDrawBg = false;
                    }
                }

                for (var index = 0; index < drawablesCount; index++)
                {
                    var drawable = drawables[index];
                    if (drawable.Visible)
                    {
                        try
                        {
                            drawable.Draw(graphics);
                        }
                        catch
                        {
                        }
                    }
                }

                if (!isBackgroundLayer) continue;
                if (disposeGraphics)
                    graphics.Dispose();
                if (_backgroundBitmap == null) continue;

                DrawBitmapWithAttributes(e.Graphics, _backgroundBitmap, _underLayerAttributes);
                needDrawBg = false;
            }

            _needInvalidate = false;
            // w.Stop(); Debug.WriteLine("Elapsed ms " + w.ElapsedMilliseconds);
        }

        private void SetUnderLayerOpacity(double value)
        {
            value = value switch
            {
                < 0 => 0,
                > 1 => 1,
                _ => value
            };
            _underLayerOpacity = value;

            var op = (float)value;
            var colorMatrix = new ColorMatrix(new[]
            {
                new[] { 1f, 0f, 0f, 0f, 0f },
                new[] { 0f, 1f, 0f, 0f, 0f },
                new[] { 0f, 0f, 1f, 0f, 0f },
                new[] { 0f, 0f, 0f, op, 0f },
                new[] { 0f, 0f, 0f, 0f, 1f }
            });

            _underLayerAttributes = new ImageAttributes();
            _underLayerAttributes.SetColorMatrix(colorMatrix, ColorMatrixFlag.Default, ColorAdjustType.Bitmap);
        }

        void ICadControlLogicOwner.TransfromChanged()
        {
            InvalidateBitmap();
            BeginInit();
            var canvasInfo = _logic.CanvasInfo;
            for (var i = _layers.Length - 1; i >= 0; i--)
            {
                var drawables = _layers[i];
                for (var index = drawables.Count - 1; index >= 0; index--)
                {
                    var drawable = drawables[index];
                    drawable.SetCanvasInfo(canvasInfo);
                }
            }

            InvalidateRequest();
            EndInit();
            OnDrawingTranformChanged();
        }

        #region properties

        public double UnderLayerOpacity
        {
            get => _underLayerOpacity;
            set
            {
                if (_underLayerOpacity.Equals(value)) return;
                SetUnderLayerOpacity(value);
                InvalidateBitmap();
                InvalidateRequest();
            }
        }

        public IDrawingPanelZoomStorage ZoomStorage
        {
            get => _logic.ZoomStorage;
            set => _logic.ZoomStorage = value;
        }

        public IBehaviorSource     BehaviorSource => _logic.BehaviorSource;
        public IDrawingColorScheme ColorScheme    { get; }

        #endregion

        public DrawingCanvasInfo CanvasInfo => _logic.CanvasInfo;

        public event EventHandler DrawingTranformChanged;

        #region Fields

        private static readonly bool cache = true;

        private Bitmap _backgroundBitmap;
        private double _underLayerOpacity;
        private ImageAttributes _underLayerAttributes;

        #endregion
    }

    partial class DrawingLayersContainer : IDrawingLayersContainer
    {
        #region properties

        public IDpHandlerContainer RootBehaviorContainer => _logic.RootBehaviorContainer;

        #endregion

        public IExtendedObservableCollection<IDrawable> Underlay  { get; }
        public IExtendedObservableCollection<IDrawable> Drawables { get; }
        public IExtendedObservableCollection<IDrawable> Overlay   { get; }

        #region Fields

        private readonly IExtendedObservableCollection<IDrawable>[] _layers;
        private readonly CadControlLogic _logic;

        #endregion
    }

    partial class DrawingLayersContainer : IInitializeableDrawingLayersContainer
    {
        public void BeginInit()
        {
            _suspendLevel++;
        }

        public void EndInit()
        {
            if (_suspendLevel < 1)
                throw new InvalidOperationException("Unable to 'end init'");
            _suspendLevel--;
            if (_suspendLevel != 0 || !_needInvalidate) return;
            Invalidate();
            _needInvalidate = false;
        }

        private void InvalidateRequest()
        {
#if USE_Debug_WriteLine
            Debug.WriteLine("DrawingLayersContainer.InvalidateRequest " + _suspendLevel);
#endif
            if (_suspendLevel > 0)
            {
                _needInvalidate = true;
                return;
            }

            Invalidate();
        }

        #region Fields

        private bool _needInvalidate;

        private int _suspendLevel;

        #endregion
    }
}
