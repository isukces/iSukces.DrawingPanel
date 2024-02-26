using System;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel.Interfaces;

public static class HandlerContainerExtensions
{
    [CanBeNull]
    public static IDisposable RegisterHandler2<T>([NotNull] this IDpHandlerContainer handlerContainer,
        [NotNull] T handler, int order)
        where T : class, IDpHandler
    {
        if (handlerContainer == null) throw new ArgumentNullException(nameof(handlerContainer));
        if (handler == null) throw new ArgumentNullException(nameof(handler));
        handlerContainer.RegisterHandler(handler, order);
        return new Registration(handlerContainer, handler);
    }

    public static void UnregisterDisposeAndSetNull<T>(this IDpHandlerContainer handlerContainer, ref T handler)
        where T : class, IDpHandler
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
        public Registration(IDpHandlerContainer container, IDpHandler handler)
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

        private IDpHandlerContainer _container;
        private IDpHandler _handler;
    }
}