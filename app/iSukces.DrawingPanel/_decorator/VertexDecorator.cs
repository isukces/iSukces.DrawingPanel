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
        : PresenterDecorator<TPresenter>, IDpHandler
        //, ITransformationScaleProvider
        where TModel : class, INotifyPropertyChanged, IVertexBasedModel
        where TThumb : IDrawable
    {
        protected VertexDecorator([NotNull] TPresenter presenter, ISnapService snap,
            [NotNull] TModel model, IDpHandlerContainer handlerContainer)
            : base(presenter)
        {
            Model                 = model ?? throw new ArgumentNullException(nameof(model));
            Snap                  = snap;
            _thumbs               = new List<TThumb>();
            _thumbsContainer      = new GroupDrawable { Name = "xcanvasForThumbs" };
            _belowThumbsContainer = new GroupDrawable { Name = "xcanvasBelowThumbs" };
            TopLevelCanvas        = new GroupDrawable { Name = "xwrapper" };
            TopLevelCanvas.Changed += (a, b) =>
            {
                OnChanged();
            };
            TopLevelCanvas.BeginInit();
            TopLevelCanvas.Children.Add(_belowThumbsContainer);
            TopLevelCanvas.Children.Add(_thumbsContainer);
            TopLevelCanvas.EndInit();

            Model.PropertyChanged     += ModelOnPropertyChanged;
            _mouseHandlerRegistration =  handlerContainer.RegisterHandler2(this, NewHandlerOrders.ElementEditorTop);
        }

        protected void AddThumb(TThumb thumb)
        {
            _thumbsContainer.Children.Add(thumb);
            _thumbs.Add(thumb);
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
            TopLevelCanvas.Draw(graphics);
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

        protected void ReduceThumbsCount(int max)
        {
            if (_thumbs.Count <= max)
                return;
            var thumbsContainerChildren = _thumbsContainer.Children;
            if (max == 0)
            {
                _thumbs.Clear();
                thumbsContainerChildren.Clear();
                return;
            }

            for (var index = thumbsContainerChildren.Count - 1; index >= max; index--)
            {
                thumbsContainerChildren.RemoveAt(index);
                _thumbs.RemoveAt(index);
            }
        }

        public override void SetCanvasInfo(DrawingCanvasInfo canvasInfo)
        {
            base.SetCanvasInfo(canvasInfo);
            TopLevelCanvas.SetCanvasInfo(canvasInfo);
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
        private readonly GroupDrawable _belowThumbsContainer;

        private readonly List<TThumb> _thumbs;
        private readonly GroupDrawable _thumbsContainer;
        protected readonly GroupDrawable TopLevelCanvas;

        protected readonly ISnapService Snap;
        private MouseEventArgs _suspendedMouseEventArgs;

        #endregion

        public sealed class ThumbSelectionChangedEventArgs
        {
            #region properties

            public TThumb Thumb { get; set; }

            #endregion
        }
        
    }
}
