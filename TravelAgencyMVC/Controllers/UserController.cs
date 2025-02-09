using Microsoft.AspNetCore.Mvc;

namespace TravelAgencyMVC.Controllers
{
    public class UserController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
