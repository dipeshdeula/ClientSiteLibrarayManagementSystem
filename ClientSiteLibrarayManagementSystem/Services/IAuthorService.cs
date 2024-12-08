using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IAuthorService
    {
      
      
        Task<IEnumerable<Author>> GetAuthorAsync(string token);
        Task<Author> GetAuthorByIdAsync(int id);

        Task<bool> AddAuthorAsync(AuthorDto author, IFormFile imageFile,string token);

        Task<bool> UpdateAuthorAsync(AuthorDto author,IFormFile authorImage,string token);
        Task<bool> DeleteAuthorAsync(int id,string token);
    }
}
