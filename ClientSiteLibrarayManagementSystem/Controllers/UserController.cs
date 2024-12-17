using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    [Route("api/[controller]")]
    public class UserController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<UserController> _logger;
        private readonly IUserService _userService;

        public UserController(IHttpContextAccessor httpContextAccessor, IHttpClientFactory httpClientFactory, ILogger<UserController> logger, IUserService userService)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _userService = userService;
        }

        /* public IActionResult Index()
         {
             return View();
         }
 */
       
        [HttpGet("UserDashboard/{UserId?}")]
        public async Task<IActionResult> UserDashboard(int? UserId = null)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Account");
            }

            // Fetch users using the token
            var users = await _userService.GetUsersAsync(token);
            User? selectedUser = null;
            if (UserId.HasValue)
            {
                selectedUser = await _userService.GetUserByIdAsync(UserId.Value);
            }

            var model = new UserViewModel
            {
                Users = users,
                User = selectedUser
            };
          //  return Json(model);
          //  return View("UserDashboard",model);
          return View(model);
        }

        [HttpPost("AddUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddUser(UserDto user, IFormFile imageFile)
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
                    _logger.LogInformation("ModelState is valid. Calling the API to register the user");
                    var result = await _userService.AddUserAsync(user, imageFile,token);

                    if (result)
                    {
                        _logger.LogInformation("User registered successfully");
                        TempData["SuccessMessage"] = "User registered successfully";
                        return RedirectToAction("UserDashboard", "User");
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

            return RedirectToAction("UserDashboard");
        }

        [HttpPost("UpdateUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateUser(UserDto user, IFormFile? imageFile)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return RedirectToAction("Login", "Account");
            }

            try {
                //if no new image is uploaded, retain the old image
                if (imageFile == null)
                {
                    var existingUser = await _userService.GetUserByIdAsync(user.UserID);
                    if (existingUser != null)
                    {
                        user.UserProfile = existingUser.UserProfile;
                    }
                }

                var result = await _userService.UpdateUserAsync(user, imageFile);
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
            return RedirectToAction("UserDashboard");

           /* if (ModelState.IsValid)
            {
                try
                {
                    _logger.LogInformation("ModelState is valid. Calling the API to update the user");
                    var result = await _userService.UpdateUserAsync(user, imageFile);

                    if (result)
                    {
                        _logger.LogInformation("User updated successfully");
                        TempData["SuccessMessage"] = "User updated successfully";
                        return RedirectToAction("UserDashboard", "User");
                    }
                    else
                    {
                        _logger.LogError("Failed to update the user");
                        ModelState.AddModelError("", "Failed to update the user");
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating the user");
                    ModelState.AddModelError("", "An error occurred while updating the user");
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
*/
            //return RedirectToAction("UserDashboard");
        }

        [HttpPost("DeleteUser")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var token = _httpContextAccessor.HttpContext?.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                // If no token is present, redirect to login
                return RedirectToAction("Login", "Account");
            }

            var result = await _userService.DeleteUserAsync(id, token);

            if (result)
            {
                _logger.LogInformation("User deleted successfully");
                TempData["SuccessMessage"] = "User deleted successfully";
            }
            else
            {
                _logger.LogError("Failed to delete the user");
                TempData["ErrorMessage"] = "Failed to delete the user";
            }

            return RedirectToAction("UserDashboard");
        }
    }
}
