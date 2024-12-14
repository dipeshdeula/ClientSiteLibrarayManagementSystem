using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    [Route("api/[controller]")]
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
        [HttpGet("AuthorDashboard/{AuthorId?}")]
        public async Task<IActionResult> AuthorDashboard(int? AuthorId = null)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                //If no token is present redirect to login
                return RedirectToAction("Login", "Account");
            }

            //Fetch authors using the token
            var authors = await _authorService.GetAuthorAsync(token);
            Author? selectedAuthor = null;
            if (AuthorId.HasValue)
            { 
                //Fetch the specific author for editing
                selectedAuthor = await _authorService.GetAuthorByIdAsync(AuthorId.Value);

            }

            var model = new AuthorViewModel
            {
                Authors = authors,
                Author = selectedAuthor
            };
            // return View(model);
            return View("AuthorDashboard", model);
        }


   
        [ValidateAntiForgeryToken]
        [HttpPost("AddAuthor")]
        public async Task<IActionResult> AddAuthor(AuthorDto author, IFormFile imageFile, string token)
        {

            if (!ModelState.IsValid)
            {

                try
                {
                    _logger.LogInformation("ModelState is valid. Calling the API to register the user");
                    var result = await _authorService.AddAuthorAsync(author, imageFile,token);
                    //return Json(result);

                    if (result)
                    {
                        _logger.LogInformation("Author registered successfully");
                        TempData["SuccessMessage"] = "Author registered successfully";
                        return RedirectToAction("AuthorDashboard", "Author");
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


            //return View("_AddAuthor", author);
            return RedirectToAction("AuthorDashboard");
        }

        [HttpPost("UpdateAuthor")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> UpdateAuthor(AuthorDto author, IFormFile? imageFile)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");

            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }
            try
            {
                // If no new image is uploaded, retain the old image
                if (imageFile == null)
                {
                    var existingAuthor = await _authorService.GetAuthorByIdAsync(author.AuthorId);
                    if (existingAuthor != null)
                    {
                        author.AuthorProfile = existingAuthor.AuthorProfile;
                    }
                }

                var result = await _authorService.UpdateAuthorAsync(author, imageFile,token);

                if (result)
                {
                    TempData["SuccessMessage"] = "Author updated successfully!";
                    return RedirectToAction("AuthorDashboard");
                }
                else
                {

                    _logger.LogError("Error updating author. Service returned false.");
                    ModelState.AddModelError("", "Error updating author");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occured while updating author");
                ModelState.AddModelError("", "An error occured while updating the author");
            }

            //return View(author);
            return RedirectToAction("AuthorDashboard");
        }

        [HttpPost("DeleteAuthor")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteAuthor(int id)
        {
            _logger.LogInformation($"DeleteAuthor method called for Author ID:{id}");
            var token = _httpContextAccessor.HttpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                _logger.LogWarning("Token is null or empty. Redirecting to Login");
                return RedirectToAction("Login", "Account");
            }
            try
            {
                var client = _httpClientFactory.CreateClient();
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);
                var response = await client.DeleteAsync($"https://localhost:7116/api/Authors/{id}");

                if (response.IsSuccessStatusCode)
                {
                    _logger.LogInformation("Author deleted successfully.");
                    TempData["SuccessMessage"] = "Author deleted successfully";
                }
                else {

                    _logger.LogError("Error deleting author. Service returned false.");
                    TempData["ErrorMessage"] = "Error deleting author.";
                }
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, "Exception occurred while deleting author.");
                TempData["ErrorMessage"] = "An error occurred while deleting the author.";
            }

            return RedirectToAction("AuthorDashboard");
        }
    }
}
