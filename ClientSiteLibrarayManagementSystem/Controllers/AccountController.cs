using ClientSiteLibrarayManagementSystem.Dtos;
using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<AccountController> _logger;
        private readonly IAuthService _authService;

        public AccountController(IHttpContextAccessor httpContextAccessor,IHttpClientFactory httpClientFactory,ILogger<AccountController> logger,IAuthService authService)
        {
            _httpContextAccessor = httpContextAccessor;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login()
        {
           

            return View();
        }

        [HttpGet]
       /*[Authorize(Roles ="Admin")]*/
       
        public IActionResult Dashboard()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Account");
            }
            return View();
        }
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                var authenticatedUser = await _authService.AuthenticateAsync(user);
                if (authenticatedUser != null && !string.IsNullOrEmpty(authenticatedUser.Token))
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, authenticatedUser.UserName ?? string.Empty),
                        new Claim(ClaimTypes.Email, authenticatedUser.Email ?? string.Empty),
                        new Claim(ClaimTypes.Role, authenticatedUser.Role ?? string.Empty)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(30)
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    HttpContext.Session.SetString("JWToken", authenticatedUser.Token);

                    return RedirectToAction("Dashboard", "Account");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            return View(user);
        }

        [HttpPost("AddUser")]
        public async Task<IActionResult> RegisterUser(UserDto user, IFormFile imageFile)
        {

            if (!ModelState.IsValid)
            {

                try
                {
                    _logger.LogInformation("ModelState is valid. Calling the API to register the user");
                    var result = await _authService.AddUserAsync(user, imageFile);
                    //return Json(result);

                    if (result)
                    {
                        _logger.LogInformation("User registered successfully");
                        TempData["SuccessMessage"] = "User registered successfully";
                        return RedirectToAction("Login", "Account");
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


            return View("_RegisterUser", user);
        }

        [AllowAnonymous]
        [HttpGet]
        public IActionResult RegisterUserForm()
        {
            var user = new UserDto();
            return PartialView("_RegisterUser", user);
        }


        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Account");
        }
    }
}
