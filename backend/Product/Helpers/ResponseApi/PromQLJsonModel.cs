﻿using System.Text.Json.Serialization;

namespace Product.Helpers.ResponseApi
{
    public class PromQLJsonModel
    {
        [JsonPropertyName("status")]
        public string Status { get; set; } = string.Empty;

        [JsonPropertyName("data")]
        public PromQLDataModel Data { get; set; }
    }

    public class PromQLDataModel
    {
        [JsonPropertyName("resultType")]
        public string ResultType { get; set; } = string.Empty;

        [JsonPropertyName("result")]
        public PromQLResultQueryModel[] Result { get; set; } =
            Array.Empty<PromQLResultQueryModel>();
    }

    public class PromQLResultQueryModel
    {
        [JsonPropertyName("metric")]
        public PromQLBaseMetric Metric { get; set; }

        [JsonPropertyName("value")]
        public object[] Value { get; set; } = Array.Empty<object>();
    }
}
