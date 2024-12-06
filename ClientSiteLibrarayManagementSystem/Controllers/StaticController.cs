using Microsoft.AspNetCore.Mvc;

namespace ClientSiteLibrarayManagementSystem.Controllers
{
    public class StaticController : Controller
    {
        public IActionResult Index()
        {
            if(User.Identity!.IsAuthenticated)
            {
                return RedirectToAction("Dashboard", "Account");
            }
            return View();
        }
    }
}
