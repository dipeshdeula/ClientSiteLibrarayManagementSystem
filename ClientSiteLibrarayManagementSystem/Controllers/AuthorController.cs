using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService _authorService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AuthorController> _logger;

        public AuthorController(IAuthorService authorService, IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, ILogger<AuthorController> logger)
        {
            _authorService = authorService;
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [HttpPost("AddAuthor")]
        public async Task<IActionResult> AddAuthor(AuthorDto author, IFormFile imageFile)
        {

            if (!ModelState.IsValid)
            {

                try
                {
                    _logger.LogInformation("ModelState is valid. Calling the API to register the user");
                    var result = await _authorService.AddAuthorAsync(author, imageFile);
                    //return Json(result);

                    if (result)
                    {
                        _logger.LogInformation("Author registered successfully");
                        TempData["SuccessMessage"] = "Author registered successfully";
                        return RedirectToAction("Dashboard", "Account");
                    }
                    else
                    {
                        _logger.LogError("Failed to register the user");
                        ModelState.AddModelError("", "Failed to register the user");
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while registering the user");
                    ModelState.AddModelError("", "An error occurred while registering the user");
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


            return View("_AddUser", author);
        }
    }
}
