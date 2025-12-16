namespace StoreManagementBlazorApp.Entities
{
    public class Category
    {
        // Phải khớp tên biến với backend: category_id, category_name
        public int category_id { get; set; }
        public string? category_name { get; set; } = "";
    }
}