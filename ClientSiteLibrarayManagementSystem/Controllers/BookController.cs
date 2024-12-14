using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class BookController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookService bookService, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, ILogger<BookController> logger)
        {
            _bookService = bookService;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet("BookDashboard/{BookId?}")]
        public async Task<IActionResult> BookDashboard(int? BookId = null)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                //If no token is present redirect to Login
                return RedirectToAction("Login", "Account");
            }

            //Fetch Books using the token
            var books = await _bookService.GetBooksAsync(token);
            Book? selectedBook = null;
            if (BookId.HasValue)
            {
                //Fetch the specific book for editing
                selectedBook = await _bookService.GetBookByIdAsync(BookId.Value);
            }

            var model = new BookViewModel
            {
                Books = books,
                Book = selectedBook
            };

            return View("BookDashboard", model);
        }
        public IActionResult Index()
        {
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost("AddBook")]
        public async Task<IActionResult> AddBook(BookDto book, string token)
        {
            if (!ModelState.IsValid)
            {
                try {
                    _logger.LogInformation("ModelState is valid. Calling the API to register the user");
                    var result = await _bookService.AddBookAsync(book, token);

                    if (result)
                    {
                        _logger.LogInformation("Book registered successfully");
                        TempData["SuccessMessage"] = "Book registered successfully";
                        return RedirectToAction("BookDashboard", "Book");
                    }
                    else {
                        _logger.LogError("failed to registered book");
                        ModelState.AddModelError("", "An error occurred while registering the book");
                    }
                   
                }
                 catch(Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while registering the book");
                    ModelState.AddModelError("", "An error occured while registering the book");

                }

            }
            else
            {
                _logger.LogWarning("ModelState is invalid.");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    _logger.LogWarning($"ModelState error: {error.ErrorMessage}");
                }
            }

            return RedirectToAction("BookDashboard");

        }

        [HttpPost("UpdateBook")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateAdd(BookDto book)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            var result = await _bookService.UpdateBookAsync(book,token);
            if (result)
            {
                TempData["SuccessMessage"] = "Book updated successfully";
                return RedirectToAction("BookDashboard");
            }
            else {
                _logger.LogError("Error on updating book . Service return false");
                ModelState.AddModelError("", "Error updating book");
            }
            return RedirectToAction("BookDashboard");
        }

        [HttpPost("DeleteBook")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteBook(int id)
        {
            _logger.LogInformation($"DeleteBook method called for book Id : {id}");
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is null or empty. Redirecting to Login");
                return RedirectToAction("Login", "Account");

            }
            try {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync($"https://localhost:7116/api/Books/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Book deleted successfully.");
                    TempData["SuccessMessage"] = "Book deleted successfully";
                }
                else
                {

                    _logger.LogError("Error deleting book. Service returned false.");
                    TempData["ErrorMessage"] = "Error deleting book.";
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting book.");
                TempData["ErrorMessage"] = "An error occurred while deleting the book.";
            }

            return RedirectToAction("BookDashboard");
        }
    }
}
