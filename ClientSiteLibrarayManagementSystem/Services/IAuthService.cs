using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IAuthService
    {
        Task<UserDto> AuthenticateAsync(User user);
        //Task<HttpResponseMessage> AddUserAsync(UserDto userDto);

       
    }
}
