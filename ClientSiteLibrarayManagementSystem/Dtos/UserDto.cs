namespace ClientSiteLibrarayManagementSystem.Dtos
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string? UserName { get; set; } 
        public string? Password { get; set; }
        public string? Email { get; set; }

        public string? UserProfile { get; set; } 
        public string? FullName { get; set; } 
        public string? Phone { get; set; } 
        public string? Role { get; set; } 

        public string? Token { get; set; } 

        public bool LoginStatus { get; set; } = false;

       // public IFormFile? UserImage { get; set; }
    }
}
