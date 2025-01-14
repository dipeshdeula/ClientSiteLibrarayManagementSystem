using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(int id);
        Task<User> GetUserByUserNameAsync(string userName, string token); // Add this method
        Task<IEnumerable<User>> GetUsersAsync(string token);
        Task<bool> AddUserAsync(UserDto user, IFormFile imageFile,string token);
        Task<bool> UpdateUserAsync(UserDto user, IFormFile imageFile);
        Task<bool> DeleteUserAsync(int id, string token);
    }
}
