using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public interface IAuthService
    {
        Task<string> AuthenticateAsync(User user);
    }
}
