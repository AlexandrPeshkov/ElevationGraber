using Newtonsoft.Json;

namespace ElevationGraber
{
    public class AltitudeRequest
    {
        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("samples")]
        public int Samples { get; set; }
    }
}
