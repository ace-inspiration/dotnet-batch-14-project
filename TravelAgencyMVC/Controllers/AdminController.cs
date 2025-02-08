using Microsoft.AspNetCore.Mvc;

namespace TravelAgencyMVC.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
