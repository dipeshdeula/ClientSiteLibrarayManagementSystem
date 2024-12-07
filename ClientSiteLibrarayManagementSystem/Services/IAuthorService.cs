using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IAuthorService
    {
      
      
        Task<IEnumerable<Author>> GetAuthorAsync(string token);
        Task<Author> GetAuthorByIdAsync(int id);

        Task<bool> AddAuthorAsync(AuthorDto author, IFormFile imageFile);

        Task<bool> UpdateAuthorAsync(AuthorDto author);
        Task<bool> DeleteAuthorAsync(int id);
    }
}
