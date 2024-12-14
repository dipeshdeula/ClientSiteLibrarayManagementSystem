using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using System.Runtime.CompilerServices;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetBooksAsync(string token);
        Task<Book> GetBookByIdAsync(int id);
        Task<bool> AddBookAsync(BookDto bookDto,string token);
        Task<bool> UpdateBookAsync(BookDto bookDto,string token);
        Task<bool> DeleteBookAsync(int id,string token);
    }
}
