namespace ClientSiteLibrarayManagementSystem.Models
{
    public class BookCopies
    {
        public int Barcode { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
    }
}
