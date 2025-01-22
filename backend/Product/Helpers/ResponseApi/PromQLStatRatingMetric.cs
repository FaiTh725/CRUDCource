using System.Text.Json.Serialization;

namespace Product.Helpers.ResponseApi
{
    public class PromQLStatRatingMetric : PromQLBaseMetric
    {
        [JsonPropertyName("ProductRatingId")]
        public string ProductId { get; set; } = string.Empty;

        [JsonPropertyName("Rating")]
        public string Rating {  get; set; } = string.Empty;
    }
}
