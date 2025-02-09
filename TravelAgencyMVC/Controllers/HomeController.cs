using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelAgencyMVC.Filters;
using TravelAgencyMVC.Models;

namespace TravelAgencyMVC.Controllers
{
    [LoginCheck]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.UserName = HttpContext.Items["UserName"];
            ViewBag.IsAdmin = HttpContext.Items["IsAdmin"];
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
