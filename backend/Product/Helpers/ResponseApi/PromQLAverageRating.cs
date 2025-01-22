using System.Text.Json.Serialization;

namespace Product.Helpers.ResponseApi
{
    public class PromQLAverageRating
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public PromQLDataAverageRating Data { get; set; }
    }

    public class PromQLDataAverageRating
    {
        [JsonPropertyName("resultType")]
        public string ResultType { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public PromQLResultQueryAverageRating[] Result { get; set; } = 
            Array.Empty<PromQLResultQueryAverageRating>();
    }

    public class PromQLResultQueryAverageRating
    {
        [JsonPropertyName("metric")]
        public PromQLProductRatingMetric Metric { get; set; }

        [JsonPropertyName("value")]
        public object[] Value { get; set; } = Array.Empty<object>();
    }
}
