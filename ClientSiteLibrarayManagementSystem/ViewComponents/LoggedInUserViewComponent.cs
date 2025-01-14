using ClientSiteLibrarayManagementSystem.Models;
using ClientSiteLibrarayManagementSystem.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ClientSiteLibrarayManagementSystem.ViewComponents
{
    public class LoggedInUserViewComponent : ViewComponent
    {
        private readonly IUserService _userService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public LoggedInUserViewComponent(IUserService userService, IHttpContextAccessor httpContextAccessor)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext == null)
            {
                return Content("HttpContext is null");
            }

            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return Content("User not logged in");
            }

            var userName = httpContext.User.Identity?.Name;
            if (string.IsNullOrEmpty(userName))
            {
                return Content("User not logged in");
            }

            var token = httpContext.Session.GetString("JWToken");
            if (string.IsNullOrEmpty(token))
            {
                return Content("Token not found");
            }

            var user = await _userService.GetUserByUserNameAsync(userName, token);
            return View(user);
        }

    }
}
