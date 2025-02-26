using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.ActivateTravelPackage;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.DeactivateTravelPackage;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.TravelersListByBookingId;
using TravelAgency.Domain.Features.TravelPackages;
using TravelAgency.Domain.Features.UserLists;
using TravelAgencyMVC.Filters;
using TravelAgencyMVC.Models;

namespace TravelAgencyMVC.Controllers;
 
[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    private readonly BookingService _bookingService;
    private readonly PaymentService _paymentService;
    private readonly ActivateTravelPackageService _activateTravelPackageService;
    private readonly DeactivateTravelPackageService _deactivateTravelPackageService;
    private readonly TravelersListService _travelersListService;
    private readonly TravelPackageService _travelPackageService;
    private readonly UserListService _userListService;
    private readonly AppDbContext _db;

    public AdminController(
        BookingService bookingService,
        PaymentService paymentService,
        ActivateTravelPackageService activateTravelPackageService,
        DeactivateTravelPackageService deactivateTravelPackageService,
        TravelPackageService travelPackageService,
        TravelersListService travelersListService,
        UserListService userListService,
        AppDbContext db)
    {
        _bookingService = bookingService;
        _paymentService = paymentService;
        _activateTravelPackageService = activateTravelPackageService;
        _deactivateTravelPackageService = deactivateTravelPackageService;
        _travelersListService = travelersListService;
        _travelPackageService = travelPackageService;
        _userListService = userListService;
        _db = db;
    }
    public async Task<IActionResult> AdminDashboard(string tab = "bookings")
    {
        //string userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value!;

        //var user = await _userListService.GetuserbyId(userId);

        var bookings = await _bookingService.GetBookingData();
        var payments = await _paymentService.GetPaymentData();
        var payment = await _paymentService.GetPayments();
        var travelPackages = await _travelPackageService.Execute();
        var userResponse = await _userListService.Execute();
        var travelerdata = await _travelersListService.Travelerdatas();
        var booking = await _bookingService.Execute();

        var Model = new AdminDashboardViewModel
        {
            Bookings = bookings,
            Payments = payments,
            Travelers = travelerdata,
            Users = userResponse,
            TravelPackages = travelPackages
        };
        ViewBag.ActiveTab = tab;

        return View(Model);
    }

    [HttpPost]
    [ActionName("SaveTravelPackage")]
    public async Task<IActionResult> SaveTravelPackage(TravelPackageRequestModel model, IFormFile? photo)
    {
        var result = await _travelPackageService.CreateTravelPackage(model, photo);

        return Json(new
        {
            success = result.Success,
            message = result.Message,
            redirectUrl = Url.Action("AdminDashboard", new { tab = "insertpackages" })
        });
    }

    [HttpGet("Admin/EditTravelPackage/{id}")]

    public async Task<IActionResult> EditTravelPackage(string id)
    {
        if (string.IsNullOrEmpty(id))
        {
            return BadRequest("ID is required.");
        }

        var travelPackage = await _travelPackageService.GetTravelPackagById(id);
        if (travelPackage == null)
        {
            return NotFound();
        }
        return View("EditTravelPackage", travelPackage);
    }


    [HttpPatch]

    [ActionName("UpdateTravelPackage")]

    public async Task<IActionResult> UpdateTravelPackage(TravelPackageRequestModel model, IFormFile? photo)
    {
        var result = await _travelPackageService.UpdateTravelPackage(model, photo);
        return Json(new
        {
            success = result.Success,
            message = result.Message,
            redirectUrl = Url.Action("AdminDashboard", new { tab = "packages" })
        });
    }


    [ActionName("ActivateTravelPackage")]
    public async Task<IActionResult> ActivateTravelPackage(string id)
    {
        var result = await _activateTravelPackageService.Execute(id);
        return Json(new
        {
            success = result.Success,
            message = result.Message,
            redirectUrl = Url.Action("AdminDashboard", "Admin", new { tab = "packages" })
        });
    }

    [ActionName("DeactivateTravelPackage")]
    public async Task<IActionResult> DeactivateTravelPackage(string id)
    {
        var result = await _deactivateTravelPackageService.Execute(id);
        return Json(new
        {
            success = result.Success,
            message = result.Message,
            redirectUrl = Url.Action("AdminDashboard", "Admin", new { tab = "packages" })
        });
    }

    [ActionName("ConfirmBooking")]
    public async Task<IActionResult> ConfirmBooking(string bookingId)
    {
        var result = await _bookingService.ConfirmBooking(bookingId);
        return Json(new
        {
            success = result.Success,
            message = result.Message,
            redirectUrl = Url.Action("AdminDashboard", "Admin", new { tab = "bookings" })
        });
    }

    [ActionName("ConfirmPayment")]
    public async Task<IActionResult> ConfirmPayment(string paymentId)
    {
        var result = await _paymentService.ConfirmPayment(paymentId);
        return Json(new
        {
            success = result.IsSuccess,
            message = result.Message,
            redirectUrl = Url.Action("AdminDashboard", "Admin", new { tab = "payments" })
        });
    }

    [HttpGet]
    [ActionName("EditTravelPackage")]
    public async Task<IActionResult> EditTravelPackage(string id)
    {
        var travelPackage = await _travelPackageService.GetTravelPackagById(id);
        if (travelPackage == null)
        {
            return NotFound();
        }

        var model = new TravelPackageRequestModel
        {
            Id = travelPackage.Id,
            Title = travelPackage.Title,
            Destination = travelPackage.Destination,
            Price = travelPackage.Price,
            Inclusions = travelPackage.Inclusions,
            CancellationPolicy = travelPackage.CancellationPolicy,
            Description = travelPackage.Description,
            Status = travelPackage.Status,
            ImageUrl = travelPackage.Image
        };

        TempData["EditPackage"] = JsonConvert.SerializeObject(model); // Store data temporarily
        return RedirectToAction("AdminDashboard", new { tab = "Editpackages" });
    }


    [HttpPost]
    [ActionName("UpdateTravelPackage")]
    public async Task<IActionResult> UpdateTravelPackage(TravelPackageRequestModel model, IFormFile? photo)
    {
        TravelPackageResponseModel result;

        if (string.IsNullOrEmpty(model.Id))
        {
            // Create new package
            result = await _travelPackageService.CreateTravelPackage(model, photo);
        }
        else
        {
            // Update existing package
            result = await _travelPackageService.UpdateTravelPackage(model, photo);
        }

        return Json(new
        {
            success = result.Success,
            message = result.Message,
            redirectUrl = Url.Action("AdminDashboard", new { tab = "packages" })
        });
    }


}

