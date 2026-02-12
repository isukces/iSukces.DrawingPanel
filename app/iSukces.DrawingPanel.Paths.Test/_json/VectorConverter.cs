using System.Globalization;
using iSukces.Mathematics;
using Newtonsoft.Json;

namespace iSukces.DrawingPanel.Paths.Test;

internal sealed class VectorConverter : Newtonsoft.Json.JsonConverter<Vector>
{
    public override void WriteJson(JsonWriter writer, Vector value, JsonSerializer serializer)
    {
        throw new NotImplementedException();
    }

    public override Vector ReadJson(JsonReader reader, Type objectType, Vector existingValue, bool hasExistingValue,
        JsonSerializer serializer)
    {
        // 48.6807545920352,42.764462268262
        var q= reader.Value.ToString().Split(',')
            .Select(a=>double.Parse(a, CultureInfo.InvariantCulture)).ToArray();
        return new Vector(q[0], q[1]);
    }
}