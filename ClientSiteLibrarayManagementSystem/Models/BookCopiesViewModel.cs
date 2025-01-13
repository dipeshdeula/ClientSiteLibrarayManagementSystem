namespace ClientSiteLibrarayManagementSystem.Models
{
    public class BookCopiesViewModel
    {
        public IEnumerable<BookCopies> BooksCopies { get; set; } = null!;
        public BookCopies BookCopy { get; set; } = null!;
        public IEnumerable<int> BooksId { get; set; } = null!; //List of book Ids
    }
}
