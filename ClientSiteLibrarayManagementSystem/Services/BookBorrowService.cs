using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using System.Net.Http.Headers;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public class BookBorrowService : IBookBorrowService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookBorrowService> _logger;

        public BookBorrowService(HttpClient httpClient, ILogger<BookBorrowService> logger)
        { 
            _httpClient = httpClient;
            _logger = logger;
        
        }
       public async Task<IEnumerable<BookBorrow>> GetBooksBorrowAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7116/api/BookBorrow");
            response.EnsureSuccessStatusCode();

            var booksBorrow = await response.Content.ReadFromJsonAsync<IEnumerable<BookBorrow>>();
            if (booksBorrow == null)
            {
                throw new InvalidOperationException("Failed to retrieve book borrow records");

            }
            return booksBorrow;
        }
        public async Task<BookBorrow> GetBookBorrowByIdAsync(int id)
        {
            var bookBorrow = await _httpClient.GetFromJsonAsync<BookBorrow>($"https://localhost:7116/api/BookBorrow/{id}");
            if (bookBorrow == null)
            {
                throw new KeyNotFoundException("book not found");
            }
            return bookBorrow;
        }
        public async Task<bool> AddBorrowBookAsync(BookBorrowDto borrowBookDtos, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.PostAsJsonAsync("https://localhost:7116/api/BookBorrow", borrowBookDtos);

            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Book borrow record added successfully.");
                return true;
            }

            else
            {
                _logger.LogError($"Failed to add book borrow record. Status code:{response.StatusCode}");
                return false;
            }
        }

       
    }
}
