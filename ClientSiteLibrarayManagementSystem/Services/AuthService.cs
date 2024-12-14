using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using System.Net.Http.Headers;
using System.Reflection;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthService> _logger;

        public AuthService(HttpClient httpClient, ILogger<AuthService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

       
        public async Task<UserDto?> AuthenticateAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7116/api/Auth/login", user);
                response.EnsureSuccessStatusCode();

                var token = await response.Content.ReadAsStringAsync();

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                var userDetailsResponse = await _httpClient.GetAsync("https://localhost:7116/api/Users");
                userDetailsResponse.EnsureSuccessStatusCode();

                var userDetailsList = await userDetailsResponse.Content.ReadFromJsonAsync<List<UserDto>>();
                var authenticatedUser = userDetailsList?.FirstOrDefault(u => u.UserName == user.UserName && u.Password == user.Password);

                if (authenticatedUser != null)
                {
                    authenticatedUser.Token = token;
                    return authenticatedUser;
                }
                _logger.LogWarning("Authentication failed for user: {UserName}", user.UserName);
                return null;

               
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating the user");
                return null;
            }
        }

        
      

      
       /* public async Task<HttpResponseMessage> RegisterUserAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7116/api/Users",userDto);
            return response;
        }
*/
      
    }
}
