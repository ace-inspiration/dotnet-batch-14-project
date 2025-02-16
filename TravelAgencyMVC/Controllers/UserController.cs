using Microsoft.AspNetCore.Mvc;
using TravelAgency.Domain.Features.TravelPackages;


namespace TravelAgencyMVC.Controllers;

public class UserController : Controller
{
    private readonly TravelPackageService _travelPackageService;
    public UserController(TravelPackageService travelPackageService)
    {
        _travelPackageService = travelPackageService;
    }   

    public IActionResult TravelPackagesList()
    {
        var lst = _travelPackageService.Execute();
        return View("TravelPackagesList", lst);
namespace TravelAgencyMVC.Controllers
{

    
    public class UserController : Controller
    {

       private readonly TravelPackageService _travelPackageService;

        public UserController (TravelPackageService travelPackageService)
        {
            _travelPackageService = travelPackageService;
        }
        public IActionResult Index()
        {
            var Lst = _travelPackageService.Execute();
            return View("Packages");
        }
    }
}
