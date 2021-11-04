using System;
using System.Globalization;

namespace iSukces.DrawingPanel
{
    public sealed partial class Ruler
    {
        private sealed class ValueToLabelConverter
        {
            public ValueToLabelConverter(double majorTickDistance)
            {
                _format = GetLabelFormat(majorTickDistance);
            }

            private static string GetLabelFormat(double majorTickDistance)
            {
                if (majorTickDistance >= 1)
                    return "N0";

                var txt      = majorTickDistance.ToString(CultureInfo.InvariantCulture);
                var dotIndex = txt.IndexOf(".", StringComparison.Ordinal);
                if (dotIndex < 0)
                    return "N0";
                dotIndex = txt.Length - dotIndex - 1;
                return "N" + dotIndex.ToInvariantString();
            }

            public string ValueToText(RulerValueAndDrawPosition value)
            {
                return Math.Abs(value.DisplayValue).ToString(_format);
            }

            private readonly string _format;
        }
    }
}