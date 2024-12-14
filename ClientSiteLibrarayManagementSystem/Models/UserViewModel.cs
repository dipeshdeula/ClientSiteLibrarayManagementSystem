namespace ClientSiteLibrarayManagementSystem.Models
{
    public class UserViewModel
    {
        public IEnumerable<User> Users { get; set; } = null!;
        public User User { get; set; } = null!;
        
    }
}
