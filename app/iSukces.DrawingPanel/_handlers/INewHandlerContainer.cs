using System;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel
{
    public interface INewHandlerContainer
    {
        void RegisterHandler(INewHandler handler, int order);
        void UnregisterHandler(INewHandler handler);
    }

    public static class HandlerContainerEx
    {
        [CanBeNull]
        public static IDisposable RegisterHandler2<T>([NotNull] this INewHandlerContainer handlerContainer,
            [NotNull] T handler, int order)
            where T : class, INewHandler
        {
            if (handlerContainer == null) throw new ArgumentNullException(nameof(handlerContainer));
            if (handler == null) throw new ArgumentNullException(nameof(handler));
            handlerContainer.RegisterHandler(handler, order);
            return new Registration(handlerContainer, handler);
        }

        public static void UnregisterDisposeAndSetNull<T>(this INewHandlerContainer handlerContainer, ref T handler)
            where T : class, INewHandler
        {
            if (handler is null)
                return;
            handlerContainer.UnregisterHandler(handler);
            // ReSharper disable once SuspiciousTypeConversion.Global
            (handler as IDisposable)?.Dispose();
            // handlerContainer.GoodByeAndDispose();
            handler = null;
        }

        private sealed class Registration : IDisposable
        {
            public Registration(INewHandlerContainer container, INewHandler handler)
            {
                _container = container;
                _handler   = handler;
            }

            ~Registration() { DisposeInternal(); }

            public void Dispose()
            {
                DisposeInternal();
                GC.SuppressFinalize(this);
            }

            private void DisposeInternal()
            {
                if (_container is null)
                    return;
                _container.UnregisterHandler(_handler);
                _container = null;
                _handler   = null;
            }

            private INewHandlerContainer _container;
            private INewHandler _handler;
        }
    }
}
