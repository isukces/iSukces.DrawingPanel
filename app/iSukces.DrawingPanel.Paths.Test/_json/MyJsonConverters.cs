using Newtonsoft.Json;

namespace iSukces.DrawingPanel.Paths.Test;

sealed class MyJsonConverters
{
    public static JsonConverter[] Get()
    {
        return
        [
            new PointConverter(),
            new VectorConverter()
        ];
    }
}
