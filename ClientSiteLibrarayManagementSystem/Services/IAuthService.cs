using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IAuthService
    {
        Task<UserDto> AuthenticateAsync(User user);
        //Task<HttpResponseMessage> AddUserAsync(UserDto userDto);

        Task<User> GetUserByIdAsync(int id);
        Task<IEnumerable<User>> GetUsersAsync(string token);

        Task<bool> AddUserAsync(UserDto user,IFormFile imageFile);

        Task<bool>UpdateUserAsync(UserDto user);
        Task<bool> DeleteUserAsync(int id);
    }
}
