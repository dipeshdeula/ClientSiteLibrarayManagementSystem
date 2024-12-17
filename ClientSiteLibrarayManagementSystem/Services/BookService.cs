using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using System.Net.Http.Headers;

namespace ClientSiteLibrarayManagementSystem.Services
{
    public class BookService : IBookService
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<BookService> _logger;
        public BookService(HttpClient httpClient, ILogger<BookService> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<Book>> GetBooksAsync(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.GetAsync("https://localhost:7116/api/Books");
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<IEnumerable<Book>>();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            var book = await _httpClient.GetFromJsonAsync<Book>($"https://localhost:7116/api/Books/{id}");
            if (book == null)
            {
                throw new KeyNotFoundException("book not found");
            }
            return book;
        }

        public async Task<bool> AddBookAsync(BookDto book, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(book.BookId.ToString()), "BookId");
            formData.Add(new StringContent(book.Title), "Title");
            formData.Add(new StringContent(book.AuthorId.ToString()), "AuthorId");
            formData.Add(new StringContent(book.Genre), "Genre");
            formData.Add(new StringContent(book.ISBN.ToString()), "ISBN");
            formData.Add(new StringContent(book.Quantity.ToString()), "Quantity");
            formData.Add(new StringContent(book.PublishedDate.ToString()), "PublishedDate");
            formData.Add(new StringContent(book.AvailabilityStatus), "AvailabilityStatus");
            

            _logger.LogInformation("sending data to API: {@formData}", formData);
            var response = await _httpClient.PostAsync("https://localhost:7116/api/Books/", formData);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Book added successfully");
                return true;
            }
            else
            { 
                _logger.LogError($"Failed to register book. Status code: {response.StatusCode}");
                return false;
            }

        }


        public async Task<bool> UpdateBookAsync(BookDto book, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var formData = new MultipartFormDataContent();
            formData.Add(new StringContent(book.BookId.ToString()), "BookId");
            formData.Add(new StringContent(book.Title), "Title");
            formData.Add(new StringContent(book.AuthorId.ToString()), "AuthorId");
            formData.Add(new StringContent(book.Genre), "Genre");
            formData.Add(new StringContent(book.ISBN.ToString()), "ISBN");
            formData.Add(new StringContent(book.Quantity.ToString()), "Quantity");
            formData.Add(new StringContent(book.PublishedDate.ToString()), "PublishedDate");
            formData.Add(new StringContent(book.AvailabilityStatus), "AvailabilityStatus");

            _logger.LogInformation("sending data to API: {@formData}", formData);
            var response = await _httpClient.PutAsync($"https://localhost:7116/api/Books/{book.BookId}", formData);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Book updated successfully");
                return true;
            }
            else
            {
                _logger.LogError($"Failed to update book. Status code: {response.StatusCode}");
                return false;
            }
        }

        public async Task<bool> DeleteBookAsync(int id, string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var response = await _httpClient.DeleteAsync($"https://localhost:7116/api/Books/{id}");

            return response.IsSuccessStatusCode;
        }

       
        

    }
}
