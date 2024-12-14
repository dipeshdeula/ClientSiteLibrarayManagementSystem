namespace ClientSiteLibrarayManagementSystem.Models
{
    public class BookViewModel
    {
        public IEnumerable<Book> Books { get; set; } = null!;
        public Book Book { get; set; } = null!;
    }
}
