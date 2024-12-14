using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using System.Net.Http.Headers;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public class AuthorService : IAuthorService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<AuthorService> _logger;

        public AuthorService(HttpClient httpClient, ILogger<AuthorService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Author>> GetAuthorAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7116/api/Authors");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Author>>();
        }

        public async Task<Author> GetAuthorByIdAsync(int id)
        {
            var author = await _httpClient.GetFromJsonAsync<Author>($"https://localhost:7116/api/Authors/{id}");
            if (author == null)
            {
                throw new KeyNotFoundException("Author not found");
            }
            return author;
        }
        public async Task<bool> AddAuthorAsync(AuthorDto author, IFormFile imageFile,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(author.AuthorId.ToString()),"AuthorId");
            formData.Add(new StringContent(author.AuthorName),"AuthorName");
            formData.Add(new StringContent(author.Biography),"Biography");

            if (imageFile != null)
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var imageContent = new ByteArrayContent(stream.ToArray());
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                    formData.Add(imageContent, "AuthorImage", imageFile.FileName);

                    //set the UserProfile property to the file name or path
                    author.AuthorProfile = imageFile.FileName;
                }
            }

            



            formData.Add(new StringContent(author.AuthorProfile), "AuthorProfile");

            _logger.LogInformation("Sending data to API: {@formData}", formData);

            var response = await _httpClient.PostAsync("https://localhost:7116/api/Authors", formData);
            //return response !=null && response.IsSuccessStatusCode;

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Author registered successfully.");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to register user. Status code: {response.StatusCode}");
                return false;
            }

        }


        public async Task<bool> UpdateAuthorAsync(AuthorDto author, IFormFile imageFile,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(author.AuthorId.ToString()), "AuthorId");
            formData.Add(new StringContent(author.AuthorName ?? string.Empty), "AuthorName");
            formData.Add(new StringContent(author.Biography?? string.Empty), "Biography");

            if (imageFile != null)
            {
                using (var stream = new MemoryStream())
                {
                    await imageFile.CopyToAsync(stream);
                    var imageContent = new ByteArrayContent(stream.ToArray());
                    imageContent.Headers.ContentType = new MediaTypeHeaderValue(imageFile.ContentType);
                    formData.Add(imageContent, "AuthorImage", imageFile.FileName);

                    //set the UserProfile property to the file name or path
                    author.AuthorProfile = imageFile.FileName;
                }
            }





           formData.Add(new StringContent(author.AuthorProfile ?? string.Empty), "AuthorProfile");

            _logger.LogInformation("Sending data to API: {@formData}", formData);

            var response = await _httpClient.PutAsync($"https://localhost:7116/api/Authors/{author.AuthorId}", formData);
            //return response !=null && response.IsSuccessStatusCode;

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Author registered successfully.");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to register user. Status code: {response.StatusCode}");
                return false;
            }

        }
        public async Task<bool> DeleteAuthorAsync(int id,string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7116/api/Authors/{id}");

            return response.IsSuccessStatusCode;
        }

       
    }
}
