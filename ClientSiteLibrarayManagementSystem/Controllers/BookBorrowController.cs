using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class BookBorrowController : Controller
    {
        private readonly IBookBorrowService _bookBorrowService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<BookBorrowController> _logger;

        public BookBorrowController(IBookBorrowService bookBorrowService, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, ILogger<BookBorrowController> logger)
        {
            _bookBorrowService = bookBorrowService;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        [HttpGet("BookBorrowTransaction/{BorrowId?}")]
        public async Task<IActionResult> BookBorrowTransaction(int? BorrowId = null)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                //If no token is present redirect to Login
                return RedirectToAction("Login", "Account");
            }

            //Fetch list of borrow books using the token
            var borrowBooks = await _bookBorrowService.GetBooksBorrowAsync(token);
            BookBorrow? selectedBook = null;
            if (BorrowId.HasValue)
            {
                //fetch the specific book for editing
                selectedBook = await _bookBorrowService.GetBookBorrowByIdAsync(BorrowId.Value);
            }

            var model = new BookBorrowViewModel
            {
                BooksBorrow = borrowBooks,
                BookBorrow = selectedBook ?? new BookBorrow() //Ensure non-null value
            };

            return View("BookBorrowTransaction", model);
        }

        [HttpGet("AddBorrowBook")]
        public IActionResult AddBorrowBook()
        {
            return View(new BookBorrowViewModel
            {
                BookBorrow = new BookBorrow()
            });
        }

        [ValidateAntiForgeryToken]
        [HttpPost("AddBorrowBook")]

        public async Task<IActionResult> AddBorrowBook(BookBorrowDto bookBorrow)
        {
            if (ModelState.IsValid)
            {
                var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
                if (string.IsNullOrEmpty(token))
                {
                    return RedirectToAction("Login", "Account");
                }

                try
                {
                    _logger.LogInformation("ModelSate is valid. Calling the API to issues the book");
                    var result = await _bookBorrowService.AddBorrowBookAsync(bookBorrow, token);

                    if (result)
                    {
                        _logger.LogInformation("Book has been borrowed successfully");
                        TempData["SuccessMessage"] = "Book has been borrowed successfully !";
                        return RedirectToAction("BookBorrowTransaction");
                    }
                    else
                    {
                        _logger.LogError("Failed to issue book");
                        ModelState.AddModelError("", "An error occurred while issuing the book");

                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occured while borrowing the book");
                    ModelState.AddModelError("", "An error occured while borrowing the book");
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

            // return View("AddBorrowBook");

            return View("AddBorrowBook", new BookBorrowViewModel
            {
                BookBorrow = new BookBorrow
                {
                    BorrowId = bookBorrow.BorrowId,
                    UserId = bookBorrow.UserId,
                    Barcode = bookBorrow.Barcode,
                    BorrowDate = bookBorrow.BorrowDate,
                    ReturnDate = bookBorrow.ReturnDate,
                    DueDate = bookBorrow.DueDate,
                    Status = bookBorrow.Status
                }
            });

        }


    }
}
