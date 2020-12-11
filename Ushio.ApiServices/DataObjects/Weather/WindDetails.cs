using Newtonsoft.Json;

namespace Ushio.ApiServices.DataObjects.Weather
{
    public class WindDetails
    {
        [JsonProperty(PropertyName = "speed")]
        public float WindSpeed { get; set; }
        [JsonProperty(PropertyName = "deg")]
        public float WindDirectionDegrees { get; set; }
    }
}
