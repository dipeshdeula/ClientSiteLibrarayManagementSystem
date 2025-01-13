using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public class BookCopiesService : IBookCopiesService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookCopiesService> _logger;
        public BookCopiesService(HttpClient httpClient, ILogger<BookCopiesService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<BookCopies>> GetBookCopiesAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7116/api/BookCopies");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<BookCopies>>();
        }

        public async Task<BookCopies> GetBookCopiesByBarcode(int id)
        {
            var bookCopies = await _httpClient.GetFromJsonAsync<BookCopies>($"https://localhost:7166/api/BookCopies/{id}");
            if (bookCopies == null)
            {
                throw new KeyNotFoundException("Book copies not found");
            }

            return bookCopies;
        }
        public async Task<bool> AddBookCopiesAsync(BookCopies bookCopies, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(bookCopies.Barcode.ToString()), "Barcode");
            formData.Add(new StringContent(bookCopies.Barcode.ToString()), "BookId");

            _logger.LogInformation("sending data to API: {@formData}", formData);
            var response = await _httpClient.PostAsync("https://localhost:7116/api/BookCopies/", formData);
            if(response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Book copies added successfully");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to add book copies. Status code: {response.StatusCode}");
                return false;
            }
        }

        public Task<bool> DeleteBookCopiesAsync(int id, string token)
        {
            throw new NotImplementedException();
        }

     

        public Task<bool> UpdateBookCopiesAsync(BookCopies bookCopies, string token)
        {
            throw new NotImplementedException();
        }
    }
}
