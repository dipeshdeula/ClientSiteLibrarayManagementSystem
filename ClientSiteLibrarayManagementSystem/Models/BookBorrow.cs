namespace ClientSiteLibrarayManagementSystem.Models
{
    public class BookBorrow
    {
        public int BorrowId { get; set; }
        public int UserId { get; set; }
        public int Barcode { get; set; }
        public DateTime? BorrowDate { get; set; }
        public DateTime? ReturnDate { get; set; }
        public DateTime? DueDate { get; set; }
        public string? Status { get; set; }


    }
}
