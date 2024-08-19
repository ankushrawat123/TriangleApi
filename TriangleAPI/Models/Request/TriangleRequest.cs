using Newtonsoft.Json;

namespace TriangleAPI.Models.Request
{
    public class TriangleRequest
    {
        [JsonProperty("firstSide")]
        public double FirstSide { get; set; }

        [JsonProperty("secondSide")]
        public double SecondSide { get; set; }

        [JsonProperty("thirdSide")]
        public double ThirdSide { get; set; }

    }
}
