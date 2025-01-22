using System.Text.Json.Serialization;

namespace Product.Helpers.ResponseApi
{
    public class PromQLBaseMetric
    {
        [JsonPropertyName("__name__")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("instance")]
        public string Instance { get; set; } = string.Empty;

        [JsonPropertyName("job")]
        public string Job { get; set; } = string.Empty;

        [JsonPropertyName("otel_scope_name")]
        public string OtelScopeName { get; set; } = string.Empty;
    }
}
