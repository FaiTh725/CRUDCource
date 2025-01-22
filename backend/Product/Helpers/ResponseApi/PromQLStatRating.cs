using System.Text.Json.Serialization;

namespace Product.Helpers.ResponseApi
{
    public class PromQLStatRating
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public PromQLDataStatRating Data { get; set; }
    }

    public class PromQLDataStatRating
    {
        [JsonPropertyName("resultType")]
        public string ResultType { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public PromQLResultQueryStatRating[] Result { get; set; } =
            Array.Empty<PromQLResultQueryStatRating>();
    }

    public class PromQLResultQueryStatRating
    {
        [JsonPropertyName("metric")]
        public PromQLStatRatingMetric Metric { get; set; }

        [JsonPropertyName("value")]
        public object[] Value { get; set; } = Array.Empty<object>();
    }
}
