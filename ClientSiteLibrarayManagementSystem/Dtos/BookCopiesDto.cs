namespace ClientSiteLibrarayManagementSystem.Dtos
{
    public class BookCopiesDto
    {
        public int Barcode { get; set; }
        public int BookId { get; set; }
        public int Quantity { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }

    }
}
