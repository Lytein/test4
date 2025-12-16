namespace StoreManagementBlazorApp.Entities
{
    public class AuthResult
    {
        public string Token { get; set; } = "";
        public string Role { get; set; } = ""; // admin | staff | customer

        public Customer? Customer { get; set; }

        public UserDto? User { get; set; }
    }

}
