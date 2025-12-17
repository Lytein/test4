using System.Text.Json.Serialization;

namespace StoreManagementBlazorApp.Entities
{
    public class Category
    {
        public int category_id { get; set; }
        public string category_name { get; set; } = "";  
    }
}
