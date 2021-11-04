using System.ComponentModel;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows;
using iSukces.DrawingPanel.Interfaces;
using Point = System.Windows.Point;
using Size = System.Drawing.Size;

namespace iSukces.DrawingPanel
{
    internal sealed class CadControlLogic : BaseDisposable
    {
        public CadControlLogic(ICadControlLogicOwner owner)
        {
            _owner             = owner;
            _behaviorContainer = new UniversalBehavior();
            var zoom = new ZoomInfo();
            _zoom = zoom;

            {
                zoom.Center = new Point(789.26833469794053, 384.41546739011932);
                zoom.Scale  = 0.25924026064589129;

                zoom.Center = new Point(34.6605311188942, 7.54494993455486);
                zoom.Scale  = 6.40188777992881;
            }
            _panAndZoom = new PanAndZoomBehavior(MouseWheelResponsibility, zoom)
            {
                DrawingSize = GetDrawingSize()
            };

            UpdateTransformAndCanvasInfo();
            // zdarzenia i rejestracje
            zoom.PropertyChanged += ZoomOnPropertyChanged;
            _behaviorContainer.RegisterHandler(_panAndZoom, NewHandlerOrders.PanAndZoom);
        }

        public void ApplyBounds(Rect bounds) { _zoom.ApplyBounds(bounds, GetDrawingSize()); }

        protected override void DisposeInternal()
        {
            IBehaviorSource src = _behaviorContainer;
            src.KeyboardFrom   = null;
            src.MouseMoveFrom  = null;
            src.MouseWheelFrom = null;
            base.DisposeInternal();
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Size GetDrawingSize() { return _owner.ClientSize; }

        public void SetDrawingSize()
        {
            _panAndZoom.DrawingSize = GetDrawingSize();
            UpdateTransformAndCanvasInfo();
            _owner.TransfromChanged();
        }

        private void UpdateTransformAndCanvasInfo()
        {
            var viewPortSize = GetDrawingSize();
            _transform = FlippedYDrawingToPixelsTransformation.Make(viewPortSize, _zoom.Center, _zoom.Scale);
            CanvasInfo = new DrawingCanvasInfo(_transform, new Rectangle(new System.Drawing.Point(), viewPortSize));
        }


        private void ZoomOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateTransformAndCanvasInfo();
            _owner.TransfromChanged();
#if false
            Debug.WriteLine("_zoom.Center = new System.Windows.Point({0}, {1});",
                _zoom.Center.X.ToInvariantString(), _zoom.Center.Y.ToInvariantString());
            Debug.WriteLine(string.Format("_zoom.Scale = {0};",
                _zoom.Scale.ToInvariantString()));
#endif
        }

        private const double MouseWheelResponsibility = 800.0;

        public IBehaviorSource   BehaviorSource => _behaviorContainer;
        public DrawingCanvasInfo CanvasInfo     { get; private set; }

        public INewHandlerContainer RootBehaviorContainer => _behaviorContainer;

        private readonly UniversalBehavior _behaviorContainer;
        private readonly ICadControlLogicOwner _owner;
        private readonly PanAndZoomBehavior _panAndZoom;
        private readonly ZoomInfo _zoom;
        private FlippedYDrawingToPixelsTransformation _transform;
    }
}
