namespace ClientSiteLibrarayManagementSystem.Dtos
{
    public class AuthorDto
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = null!;
        public string Biography { get; set; } = null!;
        public string AuthorProfile { get; set; } = null!;
    }
}
