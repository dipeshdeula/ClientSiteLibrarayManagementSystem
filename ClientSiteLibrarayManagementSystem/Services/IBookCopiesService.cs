using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IBookCopiesService
    {
        Task<IEnumerable<BookCopies>> GetBookCopiesAsync(string token);
        Task<BookCopies> GetBookCopiesByBarcode(int id);
        Task<bool> AddBookCopiesAsync(BookCopies bookCopies, string token);
        Task<bool> UpdateBookCopiesAsync(BookCopies bookCopies, string token);
        Task<bool> DeleteBookCopiesAsync(int id, string token);
    }
}

