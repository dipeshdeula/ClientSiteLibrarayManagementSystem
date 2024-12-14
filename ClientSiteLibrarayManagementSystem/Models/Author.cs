namespace ClientSiteLibrarayManagementSystem.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string? AuthorName { get; set; } 
        public string? Biography { get; set; }
        public string? AuthorProfile { get; set; } = "abc.png";

        public IFormFile? AuthorImage { get; set; }
    }
}
