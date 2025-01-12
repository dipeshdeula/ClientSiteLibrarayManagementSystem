using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IBookBorrowService
    {
        Task<IEnumerable<BookBorrow>> GetBooksBorrowAsync(string token);
        Task<BookBorrow> GetBookBorrowByIdAsync(int id);
        Task<bool> AddBorrowBookAsync(BookBorrowDto borrowBookDtos, string token);



    }
}
