using System;
using System.Text.Json.Serialization;

namespace StoreManagementBlazorApp.Entities
{
    public class Customer
    {
        [JsonPropertyName("customer_id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string name { get; set; } = "";

        [JsonPropertyName("email")]
        public string email { get; set; } = "";

        [JsonPropertyName("phone")]
        public string phone { get; set; } = "";

        [JsonPropertyName("address")]
        public string address { get; set; } = "";

        [JsonPropertyName("created_at")]
        public DateTime created_at { get; set; }
    }

}
