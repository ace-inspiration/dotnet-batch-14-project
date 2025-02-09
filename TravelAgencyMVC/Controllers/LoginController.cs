using Microsoft.AspNetCore.Mvc;

namespace TravelAgencyMVC.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
