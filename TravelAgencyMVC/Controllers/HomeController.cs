using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Security.Claims;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.TravelPackages;
using TravelAgencyMVC.Models;




namespace TravelAgencyMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly TravelPackageService _travelPackageService;
    private readonly BookingService _bookingService;
    

    public HomeController(TravelPackageService travelPackageService,BookingService bookingService)
    {
        _travelPackageService = travelPackageService;
        _bookingService = bookingService;
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

    public async Task<IActionResult> Packages()
    {
        var lst = await _travelPackageService.Execute();
        return View("Packages", lst);
    }

    public async Task<IActionResult> PackageDetail(string id)
    {

        var item = await _travelPackageService.GetTravelPackagById(id);
        return View("PackageDetail", item);
    }

    //public async Task<IActionResult> BookingHistory()
    //{
    //    var booking_lst = await _bookingService.Execute();
    //    var package_lst = await _travelPackageService.Execute();

    //    var viewModel = new BookingHistoryViewModel
    //    {
    //        Bookings = booking_lst,
    //        Packages = package_lst
    //    };

    //    return View(viewModel);
    //}

    public async Task<IActionResult> BookingHistory()
    {
        var userId = User.FindFirstValue("UserId");
        Console.WriteLine($"UserId: {userId}");
        var lst = await _bookingService.GetBookingDataByUserId(userId);
        Console.WriteLine($"Booking Count: {lst.Count}");
        return View("BookingHistory", lst);
    }




    //public async Task<IActionResult> Payment(string id)
    //{
    //    var item = await _travelPackageService.GetTravelPackagById(id); 

    //    return View("Payment",item);
    //}   

    //After finishing of creating booking we will user above method but for now we just use follwing method

    public IActionResult Payment ()
    {
        return View("Payment");
    }

}


