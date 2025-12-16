using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace StoreManagementBlazorApp.Entities
{
    [Table("Products")]
    public class Product
    {
        [Key]
        [JsonPropertyName("product_id")]
        public int Id { get; set; }

        [Required, MaxLength(100)]
        [JsonPropertyName("product_name")]
        public string Name { get; set; } = "";

        [MaxLength(500)]
        [JsonPropertyName("barcode")]
        public string? bar_code { get; set; } = "";

        [MaxLength(50)]
        public string Category { get; set; } = "";

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
        [JsonPropertyName("unit")]
        public string? unit { get; set; }
        public int Stock { get; set; }

        [MaxLength(200)]
        [JsonPropertyName("image_url")]
        public string Image { get; set; } = "";
        [JsonPropertyName("created_at")]
        public DateTime created_at { get; set; }
    }
}
