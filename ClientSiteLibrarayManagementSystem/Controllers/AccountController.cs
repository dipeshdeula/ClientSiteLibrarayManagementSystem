using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;

        public AccountController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]

        public IActionResult Login()
        {
            if (User.Identity!.IsAuthenticated)
            { 
                return RedirectToAction("Dashboard", "Home");
            }

            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Login(User user)
        {
            if (!ModelState.IsValid)
            {
                //Authenticate and retrieve the token
                string token = await _authService.AuthenticateAsync(user);

                if (!string.IsNullOrEmpty(token))
                {
                    //store the token in session
                    HttpContext.Session.SetString("JWToken", token);

                    //Redirect to the dashboard on successful login
                    return RedirectToAction("index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt");
            }
            //return Json(user);
            return View(user);
        }
    }
}
