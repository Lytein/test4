using System.Text.Json.Serialization;

namespace StoreManagementBlazorApp.Entities
{
    public class Promotion
    {
        [JsonPropertyName("promo_id")]
        public int PromoId { get; set; }

        [JsonPropertyName("promo_code")]
        public string PromoCode { get; set; } = null!;

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        // percent | fixed
        [JsonPropertyName("discount_type")]
        public string DiscountType { get; set; } = null!;

        [JsonPropertyName("discount_value")]
        public decimal DiscountValue { get; set; }

        [JsonPropertyName("start_date")]
        public DateTime StartDate { get; set; }

        [JsonPropertyName("end_date")]
        public DateTime EndDate { get; set; }

        [JsonPropertyName("min_order_amount")]
        public decimal MinOrderAmount { get; set; }

        [JsonPropertyName("usage_limit")]
        public int UsageLimit { get; set; }

        [JsonPropertyName("used_count")]
        public int UsedCount { get; set; }

        // active | inactive
        [JsonPropertyName("status")]
        public string Status { get; set; } = null!;
    }
}
