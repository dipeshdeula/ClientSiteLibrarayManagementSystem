namespace ClientSiteLibrarayManagementSystem.Models
{
    public class Book
    {
        public int BookId { get; set; }
        public string? Title { get; set; } 
        public int AuthorId { get; set; }
        public string? Genre { get; set; } 
        public string? ISBN { get; set; } 
        public int Quantity { get; set; }
        public DateOnly PublishedDate { get; set; }
        public string AvailabilityStatus { get; set; } = "Available";
    }
}
