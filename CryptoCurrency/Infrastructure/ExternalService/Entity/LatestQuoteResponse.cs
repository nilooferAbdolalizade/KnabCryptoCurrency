using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Infrastructure.ExternalService.Entity
{
    public class LatestQuoteResponse
    {
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        [JsonPropertyName("data")]
        public Dictionary<string, Data> Data { get; set; }
    }
    public class Status
    {
        [JsonPropertyName("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonPropertyName("error_code")]
        public int ErrorCode { get; set; }

        [JsonPropertyName("error_message")]
        public object ErrorMessage { get; set; }

        [JsonPropertyName("elapsed")]
        public int Elapsed { get; set; }

        [JsonPropertyName("credit_count")]
        public int CreditCount { get; set; }

        [JsonPropertyName("notice")]
        public object Notice { get; set; }
    }

    public class Quote
    {
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }

    public class Data
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }

        [JsonPropertyName("quote")]
        public Dictionary<string, Quote> Quote { get; set; }
    }
}
