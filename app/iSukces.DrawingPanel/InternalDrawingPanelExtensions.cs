using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace iSukces.DrawingPanel
{
    internal static class InternalDrawingPanelExtensions
    {
        [Pure]
        public static TOut[] MapToArray<TIn, TOut>(this IList<TIn> list, Func<TIn, TOut> map)
        {
            if (list is null || list.Count == 0)
                return Array.Empty<TOut>();

            var result = new TOut[list.Count];
            // ReSharper disable once LoopCanBeConvertedToQuery
            for (var i = 0; i < list.Count; i++)
            {
                var element = list[i];
                var value   = map(element);
                result[i] = value;
            }

            return result;
        }

        [CanBeNull]
        [Pure]
        public static string TrimToNull(this string value)
        {
            value = value?.Trim();
            return string.IsNullOrEmpty(value) ? null : value;
        }
    }
}
