using Microsoft.AspNetCore.Mvc;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.ActivateTravelPackage;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.DeactivateTravelPackage;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.TravelersListByBookingId;
using TravelAgency.Domain.Features.TravelPackages;
using TravelAgency.Domain.Features.UserLists;
using TravelAgencyMVC.Models;

namespace TravelAgencyMVC.Controllers;

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
    public async Task<IActionResult> AdminDashboard()
    {
        var bookings = await _bookingService.GetBookingData();
        var payments = await _paymentService.GetPaymentData();
        var payment = await _paymentService.GetPayments();
        var travelPackages = await _travelPackageService.Execute();
        var userResponse = await _userListService.Execute();
        var travelers = await _travelersListService.GetTravelers();
        var booking = await _bookingService.GetBookings();

        var Model = new AdminDashboardViewModel
        {
            Bookings = bookings,
            Payments = payments,
            Travelers = travelers,
            Users = userResponse,
            TravelPackages = travelPackages,
        };
        return View("AdminDashboard", Model);
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
            redirectUrl = Url.Action("AdminDashboard", new { tab = "packages" })
        });
    }


    [ActionName("ActivateTravelPackage")]
    public async Task<IActionResult> ActivateTravelPackage(string id)
    {
        var result = await _activateTravelPackageService.Execute(id);
        return Json(new { success = result.Success, message = result.Message, redirectUrl = Url.Action("AdminDashboard", new { tab = "packages" }) });
    }

    [ActionName("DeactivateTravelPackage")]
    public async Task<IActionResult> DeactivateTravelPackage(string id)
    {
        var result = await _deactivateTravelPackageService.Execute(id);
        return Json(new { success = result.Success, message = result.Message, redirectUrl = Url.Action("AdminDashboard", new { tab = "packages" }) });
    }

    [ActionName("ConfirmBooking")]
    public async Task<IActionResult> ConfirmBooking(string bookingId)
    {
        var result = await _bookingService.ConfirmBooking(bookingId);
        return Json(new { success = result.Success, message = result.Message, redirectUrl = Url.Action("AdminDashboard", new { tab = "bookings" }) });
    }

    [ActionName("ConfirmPayment")]
    public async Task<IActionResult> ConfirmPayment(string paymentId)
    {
        var result = await _paymentService.ConfirmPayment(paymentId);
        return Json(new { success = result.IsSuccess, message = result.Message, redirectUrl = Url.Action("AdminDashboard", new { tab = "payments" }) });
    }



}

