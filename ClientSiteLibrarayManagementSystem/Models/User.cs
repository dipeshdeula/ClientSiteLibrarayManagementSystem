namespace ClientSiteLibrarayManagementSystem.Models
{
    public class User
    {
        public int UserID { get; set; }
        public string? UserName { get; set; } 
        public string? Password { get; set; } 
        public string? Email { get; set; } 

        public string? UserProfile { get; set; }
        public string? FullName { get; set; } 
        public string? Phone { get; set; } = null!;
        public string? Role { get; set; }

       // public string? Token { get; set; }

        public bool LoginStatus { get; set; } = false;
    }
}
