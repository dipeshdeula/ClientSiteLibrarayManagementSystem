using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class BookCopiesController : Controller
    {
        private readonly IBookCopiesService _bookCopiesService;
        private readonly IBookService _bookService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookCopiesController> _logger;

        public BookCopiesController(IBookCopiesService bookCopiesService, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, ILogger<BookCopiesController> logger,IBookService bookService)
        {
            _bookCopiesService = bookCopiesService;
            _bookService = bookService;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet("BookCopiesDashboard/{Barcode?}")]
        public async Task<IActionResult> BookCopiesDashboard(int? Barcode = null)
        { 
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if(string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            //Fetch Book Copies using the token
            var bookCopies = await _bookCopiesService.GetBookCopiesAsync(token);
            var books = await _bookService.GetBooksAsync(token); // fetch the list of book IDs
            var bookIds = books.Select(b => b.BookId); //Extract the list of book IDs
            BookCopies? selectedBookCopies = null;
            if (Barcode.HasValue)
            {
                //Fetch the specific book copies for editing
                selectedBookCopies = await _bookCopiesService.GetBookCopiesByBarcode(Barcode.Value);
            }

            var model = new BookCopiesViewModel
            {
                BooksCopies = bookCopies,
                BookCopy = selectedBookCopies,
                BooksId = bookIds // Pass the list of book IDs to the view

            };

            return View("BookCopiesDashboard", model);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
