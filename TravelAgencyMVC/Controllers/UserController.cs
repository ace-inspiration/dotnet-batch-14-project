using Microsoft.AspNetCore.Mvc;
using TravelAgency.Domain.Features.TravelPackages;


namespace TravelAgencyMVC.Controllers;

    public class UserController : Controller
    {

       private readonly TravelPackageService _travelPackageService;

        public UserController (TravelPackageService travelPackageService)
        {
            _travelPackageService = travelPackageService;
        }
        public async Task<IActionResult> Index()
        {
            var Lst = await _travelPackageService.Execute();
            return View("Packages", Lst);
        }
    }

