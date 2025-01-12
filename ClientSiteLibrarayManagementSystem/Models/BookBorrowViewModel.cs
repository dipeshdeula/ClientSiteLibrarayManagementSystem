namespace ClientSiteLibrarayManagementSystem.Models
{
    public class BookBorrowViewModel
    {
        public IEnumerable<BookBorrow> BooksBorrow { get; set; } = null!;
        public BookBorrow BookBorrow { get; set; } = null!;
    }
}
