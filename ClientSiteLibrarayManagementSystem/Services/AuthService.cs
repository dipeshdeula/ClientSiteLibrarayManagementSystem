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

       
        public async Task<UserDto> AuthenticateAsync(User user)
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
                }

                return authenticatedUser;
            }
            catch (HttpRequestException ex)
            {
                _logger.LogError(ex, "An error occurred while authenticating the user");
                return null;
            }
        }

        public async Task<IEnumerable<User>> GetUsersAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7116/api/Users");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<User>>();

        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = await _httpClient.GetFromJsonAsync<User>($"https://localhost:7116/api/Users/{id}");
            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }
            return user;
        }

        public async Task<bool> AddUserAsync(UserDto user, IFormFile imageFile)
        {
           // _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(user.UserID.ToString()), "UserID");
            formData.Add(new StringContent(user.UserName), "UserName");
            formData.Add(new StringContent(user.Password), "Password");
            formData.Add(new StringContent(user.Email), "Email");
            formData.Add(new StringContent(user.FullName), "FullName");
            formData.Add(new StringContent(user.Phone), "Phone");
            formData.Add(new StringContent(user.Role), "Role");

            if( imageFile!=null)
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var imageContent  = new ByteArrayContent(stream.ToArray());
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                    formData.Add(imageContent, "UserImage", imageFile.FileName);

                    //set the UserProfile property to the file name or path
                    user.UserProfile = imageFile.FileName;
                }
            }

            formData.Add(new StringContent(user.UserProfile), "UserProfile");

            _logger.LogInformation("Sending data to API: {@formData}", formData);

            var response = await _httpClient.PostAsync("https://localhost:7116/api/Users", formData);
            //return response !=null && response.IsSuccessStatusCode;

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("User registered successfully.");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to register user. Status code: {response.StatusCode}");
                return false;
            }
        }


        public Task<bool> UpdateUserAsync(UserDto user)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

      

      
       /* public async Task<HttpResponseMessage> RegisterUserAsync(UserDto userDto)
        {
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7116/api/Users",userDto);
            return response;
        }
*/
      
    }
}
