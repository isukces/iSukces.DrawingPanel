using System.Globalization;
using Newtonsoft.Json;
using Point = iSukces.Mathematics.Point;

namespace iSukces.DrawingPanel.Paths.Test;

internal sealed class PointConverter : Newtonsoft.Json.JsonConverter<Point>
{
    public override void WriteJson(JsonWriter writer, Point value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Point ReadJson(JsonReader reader, Type objectType, Point existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        // 48.6807545920352,42.764462268262
        var q= reader.Value.ToString().Split(',')
            .Select(a=>double.Parse(a, CultureInfo.InvariantCulture)).ToArray();
        return new Point(q[0], q[1]);
    }
}
