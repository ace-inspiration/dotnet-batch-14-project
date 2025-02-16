using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TravelAgency.Domain.Features.TravelPackages;
using TravelAgencyMVC.Models;

namespace TravelAgencyMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    
    private readonly TravelPackageService _travelPackageService;

   public HomeController (TravelPackageService travelPackageService)
    {
        _travelPackageService = travelPackageService;
    }

    public IActionResult Index()
    {
        if (User.IsInRole("admin"))
        {
            return RedirectToAction("AdminDashboard", "Admin");
        }

        ViewBag.UserName = User.Identity.Name;
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult AccessDenied()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

    public IActionResult Packages ()
    {
        var lst = _travelPackageService.Execute();
        return View("Packages", lst);
    }
}
