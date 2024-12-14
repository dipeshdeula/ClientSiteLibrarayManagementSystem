using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using System.Net.Http.Headers;
using System.Net.Http;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<UserService> _logger;

        public UserService(HttpClient httpClient, ILogger<UserService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
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
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(user.UserID.ToString()), "UserID");
            formData.Add(new StringContent(user.UserName), "UserName");
            formData.Add(new StringContent(user.Password), "Password");
            formData.Add(new StringContent(user.Email), "Email");
            formData.Add(new StringContent(user.FullName), "FullName");
            formData.Add(new StringContent(user.Phone), "Phone");
            formData.Add(new StringContent(user.Role), "Role");

            if (imageFile != null)
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var imageContent = new ByteArrayContent(stream.ToArray());
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                    formData.Add(imageContent, "UserImage", imageFile.FileName);

                    user.UserProfile = imageFile.FileName;
                }
            }

            formData.Add(new StringContent(user.UserProfile), "UserProfile");

            _logger.LogInformation("Sending data to API: {@formData}", formData);

            var response = await _httpClient.PostAsync("https://localhost:7116/api/Users", formData);

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

        public async Task<bool> UpdateUserAsync(UserDto user, IFormFile imageFile)
        {
            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(user.UserID.ToString()), "UserID");
            formData.Add(new StringContent(user.UserName), "UserName");
            formData.Add(new StringContent(user.Password), "Password");
            formData.Add(new StringContent(user.Email), "Email");
            formData.Add(new StringContent(user.FullName), "FullName");
            formData.Add(new StringContent(user.Phone), "Phone");
            formData.Add(new StringContent(user.Role), "Role");

            if (imageFile != null)
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var imageContent = new ByteArrayContent(stream.ToArray());
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                    formData.Add(imageContent, "UserImage", imageFile.FileName);

                    user.UserProfile = imageFile.FileName;
                }
            }

            formData.Add(new StringContent(user.UserProfile), "UserProfile");

            _logger.LogInformation("Sending data to API: {@formData}", formData);

            var response = await _httpClient.PutAsync($"https://localhost:7116/api/Users/{user.UserID}", formData);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("User updated successfully.");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to update user. Status code: {response.StatusCode}");
                return false;
            }
        }

        public async Task<bool> DeleteUserAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7116/api/Users/{id}");

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("User deleted successfully.");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to delete user. Status code: {response.StatusCode}");
                return false;
            }
        }
    }
}
