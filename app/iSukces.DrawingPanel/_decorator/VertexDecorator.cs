using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using iSukces.DrawingPanel.Interfaces;
using JetBrains.Annotations;
using WinPoint = System.Windows.Point;

namespace iSukces.DrawingPanel
{
    //   [UmlDiagram(UmlDiagrams.CadBusUiControls)]
    public abstract class VertexDecorator<TModel, TThumb, TPresenter>
        : PresenterDecorator<TPresenter> //, ITransformationScaleProvider
        where TModel : class, INotifyPropertyChanged, IVertexBasedModel
    {
        protected VertexDecorator([NotNull] TPresenter presenter, ISnapService snap,
            [NotNull] TModel model)
            : base(presenter)
        {
            Model                 = model ?? throw new ArgumentNullException(nameof(model));
            Snap                  = snap;
            _thumbs               = new List<TThumb>();
            _thumbsContainer      = new GroupDrawable { Name = "xcanvasForThumbs" };
            _belowThumbsContainer = new GroupDrawable { Name = "xcanvasBelowThumbs" };
            _topLevelCanvas       = new GroupDrawable { Name = "xwrapper" };
            _topLevelCanvas.Changed += (a, b) =>
            {
                OnChanged();
            };
            _topLevelCanvas.BeginInit();
            _topLevelCanvas.Children.Add(_belowThumbsContainer);
            _topLevelCanvas.Children.Add(_thumbsContainer);
            _topLevelCanvas.EndInit();

            Model.PropertyChanged += ModelOnPropertyChanged;
        }

        /// <summary>
        ///     Metoda wywoływana kiedy skończyło się przesuwanie thumbów a był jakikolwiek ruch
        /// </summary>
        protected virtual void AfterThumbDragWithMoveCompleted()
        {
        }


        protected override void DisposeInternal(bool disposing)
        {
            _mouseHandlerRegistration?.Dispose();
            _mouseHandlerRegistration =  null;
            Model.PropertyChanged     -= ModelOnPropertyChanged;
        }

        public override void Draw(Graphics graphics)
        {
            _topLevelCanvas.Draw(graphics);
        }

        protected void HandleThumbIsSelectedChanged(object sender, EventArgs e)
        {
            var handle = ThumbSelectionChanged;
            if (handle is null)
                return;
            var args = new ThumbSelectionChangedEventArgs
            {
                Thumb = (TThumb)sender
            };
            handle(this, args);
        }

        protected virtual void ModelOnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
        }

        private void ReduceThumbsCount(int max)
        {
            if (_thumbs.Count <= max)
                return;
            var thumbsContainerChildren = _thumbsContainer.Children;
            if (max == 0)
            {
                _thumbs.Clear();
                thumbsContainerChildren.Clear();
            }
            else
            {
                for (var index = thumbsContainerChildren.Count - 1; index >= max; index--)
                {
                    thumbsContainerChildren.RemoveAt(index);
                    _thumbs.RemoveAt(index);
                }
            }
        }

        public override void SetCanvasInfo(DrawingCanvasInfo canvasInfo)
        {
            base.SetCanvasInfo(canvasInfo);
            _topLevelCanvas.SetCanvasInfo(canvasInfo);
        }

        protected abstract bool ThumbDragDelta(TThumb thumb, ref WinPoint worldPoint);

        #region properties

        [NotNull]
        protected TModel Model { get; }

        public IReadOnlyList<TThumb> Thumbs => _thumbs;

        #endregion


        /// <summary>
        ///     Odpala się jeśli któryś z thumbów zmienił IsSelected
        /// </summary>
        public event EventHandler<ThumbSelectionChangedEventArgs> ThumbSelectionChanged;

        #region Fields

        private IDisposable _mouseHandlerRegistration;
        protected readonly GroupDrawable _belowThumbsContainer;

        private readonly List<TThumb> _thumbs;
        private readonly GroupDrawable _thumbsContainer;
        private readonly GroupDrawable _topLevelCanvas;

        protected readonly ISnapService Snap;
        private MouseEventArgs _suspendedMouseEventArgs;

        #endregion

        public sealed class ThumbSelectionChangedEventArgs
        {
            #region properties

            public TThumb Thumb { get; set; }

            #endregion
        }


        /*
        public delegate void ArrangeThumbDelegate(string reason, PdFormsThumb thumb);

        public delegate void ArrangeThumbDelegateShort(PdFormsThumb thumb);*/


        /*public DrawingHandleResult HandleOnMouseDown(MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left) return DrawingHandleResult.Continue;

            var index = ThumbsExtensions.FindThumb(Thumbs, e.Location);
            if (index < 0) return DrawingHandleResult.Continue;
#if DEBUG && LOG
            PdLog2.Default.Info(GetType().Name, $"OnDragStarted {index}, {e.Location}");
#endif
            var thumb = Thumbs[index];
            var point = CanvasInfo.Transformation.FromCanvas(e.Location);
            _thumbLogic.OnDragInit(thumb, point);
#if DEBUG && LOG
            PdLog2.Default.Debug(nameof(VertexAdorner<TModel>), "Drag started " + _thumbLogic.DraggedStartingCounter);
#endif
            return DrawingHandleResult.Break;
        }*/
    }
}
