namespace StoreManagementBlazorApp.Entities
{
    public class UserDto
    {
        public int User_Id { get; set; }
        public string Username { get; set; } = "";
        public string Password { get; set; } = "";
        public string Full_Name { get; set; } = "";
        public string Role { get; set; }
        public DateTime Created_At { get; set; }
    }

}
