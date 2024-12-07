namespace ClientSiteLibrarayManagementSystem.Dtos
{
    public class UserDto
    {
        public int UserID { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = null!;
        public string Email { get; set; } = string.Empty;

        public string UserProfile { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string Role { get; set; } = string.Empty;

        public string Token { get; set; } = string.Empty;

        public bool LoginStatus { get; set; } = false;

       // public IFormFile? UserImage { get; set; }
    }
}
