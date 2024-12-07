namespace ClientSiteLibrarayManagementSystem.Models
{
    public class UserViewModel
    {
        public User User { get; set; } = new User();
        public IEnumerable<User> Users { get; set; } = new List<User>();
    }
}
