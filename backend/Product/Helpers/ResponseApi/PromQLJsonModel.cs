using System.Text.Json.Serialization;

namespace Product.Helpers.ResponseApi
{
    public class PromQLJsonModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public Data Data { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("resultType")]
        public string ResultType { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public ResultQuery[] Result { get; set; } = Array.Empty<ResultQuery>();
    }

    public class ResultQuery
    {
        [JsonPropertyName("metric")]
        public Metric Metric { get; set; }

        [JsonPropertyName("value")]
        public object[] Value { get; set; } = Array.Empty<object>();
    }

    public class Metric
    {
        [JsonPropertyName("ProductId")]
        public string ProductId { get; set; } = string.Empty;

        [JsonPropertyName("__name__")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("instance")]
        public string Instance { get; set; } = string.Empty;

        [JsonPropertyName("job")]
        public string Job { get; set; } = string.Empty;

        [JsonPropertyName("otel_scope_name")]
        public string OtelScopeName {  get; set; } = string.Empty;
    }
}
