using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.VisualBasic;
using System.Diagnostics;
using System.Security.Claims;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.TravelPackages;
using TravelAgencyMVC.Models;




namespace TravelAgencyMVC.Controllers;

[Authorize]
public class HomeController : Controller
{
    private readonly TravelPackageService _travelPackageService;
    private readonly BookingService _bookingService;
    private readonly PaymentService _paymentService;
    

    public HomeController(TravelPackageService travelPackageService,BookingService bookingService, PaymentService paymentService)
    {
        _travelPackageService = travelPackageService;
        _bookingService = bookingService;
        _paymentService = paymentService;
    }

    public async Task<IActionResult> Index()
    {
        if (User.IsInRole("admin"))
        {
            return RedirectToAction("AdminDashboard", "Admin");
        }
        var lst = await _travelPackageService.GetPopularTravelPackage();
        ViewBag.UserName = User.Identity.Name;
        return View(lst);
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


    public async Task<IActionResult> BookingHistory()
    {
        var userId = User.FindFirstValue("UserId");
   
        var lst = await _bookingService.GetBookingDataByUserId(userId);
        
        return View("BookingHistory", lst);
    }

    public async Task<IActionResult> CreateBooking([FromBody] BookingRequestModel requestModel)
    {
        var response = await _bookingService.Execute(requestModel);
        return Json(new
        {
            success = response.Success,
            message = response.Message,
            redirectUrl = Url.Action("BookingHistory", "Home")
        });
    }


    [HttpGet]
    public async Task<IActionResult> DeleteBooking(string bookingId)
    {
        if (string.IsNullOrEmpty(bookingId))
        {
            return NotFound();
        }

        // Call your service to delete the booking
        var result = await _bookingService.DeleteBookingHistory(bookingId);

       

        // After deletion, redirect to the booking history page
        return RedirectToAction("BookingHistory");
    }



    public async Task<IActionResult> Payment(string bookingId)
    {
        var item = await _bookingService.GetBookingDataByBookingId(bookingId);
        Console.Write(item);
        return View("Payment",item);
    }


   public async Task<IActionResult> PaymentHistory (string bookingId)
    {
        var item = await _bookingService.GetBookingDataByBookingId(bookingId);
        Console.Write(item);
        return View("PaymentHistory", item);
    }


}


