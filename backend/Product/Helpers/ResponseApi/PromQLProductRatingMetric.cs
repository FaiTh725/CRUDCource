using System.Text.Json.Serialization;

namespace Product.Helpers.ResponseApi
{
    public class PromQLProductRatingMetric : PromQLBaseMetric
    {
        [JsonPropertyName("ProductId")]
        public string ProductId { get; set; } = string.Empty;
    }
}
