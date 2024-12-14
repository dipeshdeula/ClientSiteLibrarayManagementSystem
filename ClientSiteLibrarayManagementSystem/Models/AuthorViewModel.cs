namespace ClientSiteLibrarayManagementSystem.Models
{
    public class AuthorViewModel
    {
        public IEnumerable<Author> Authors { get; set; } = null!;
        public Author Author { get; set; } = null!;
       
    }
}
