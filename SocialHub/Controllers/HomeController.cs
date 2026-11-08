using Microsoft.AspNetCore.Mvc;

namespace SocialHub.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            //return RedirectToAction("Index", "Post");
            return View();
        }
    }
}
