namespace ClientSiteLibrarayManagementSystem.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string Title { get; set; } = string.Empty;
        public int AuthorId { get; set; }
        public string Genre { get; set; } = string.Empty;
        public string ISBN { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public DateOnly PublishedDate { get; set; }
        public string AvailabilityStatus { get; set; } = string.Empty;
    }
}
