#if UNUSED
using System;

namespace iSukces.DrawingPanel
{
    public static class HandlerContainerEx
    {
        public static void UnregisterDisposeAndSetNull<T>(this IHandlerContainer handlerContainer, ref T handler)
            where T : class, IHandler
        {
            if (handler == null)
                return;
            handlerContainer.UnregisterHandler(handler);
            // ReSharper disable once SuspiciousTypeConversion.Global
            if (handlerContainer is IDisposable disposable)
                disposable.Dispose();
            handler = null;
        }
    }
}
#endif