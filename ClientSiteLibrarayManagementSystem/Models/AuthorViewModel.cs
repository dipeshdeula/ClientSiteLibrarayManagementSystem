namespace ClientSiteLibrarayManagementSystem.Models
{
    public class AuthorViewModel
    {
        public Author Author { get; set; } = null!;
        public IEnumerable<Author> Authors { get; set; } = null!;
    }
}
