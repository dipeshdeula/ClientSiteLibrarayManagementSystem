using ClientSiteLibrarayManagementSystem.Models;

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

        public async Task<string> AuthenticateAsync(User user)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("https://localhost:7116/api/Auth/login", user);
                response.EnsureSuccessStatusCode();

                var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
               
                return tokenResponse?.Token;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating the user");
                return null;
            }
        }
    }
}
