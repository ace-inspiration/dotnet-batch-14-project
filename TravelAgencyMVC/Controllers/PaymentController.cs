﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.TravelPackages;
using TravelAgency.Domain.Features.UserLists;
using TravelAgencyMVC.Models;

namespace TravelAgencyMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
		private readonly BookingService _bookingService;
		private readonly TravelPackageService _travelPackageService;
		private readonly UserListService _userListService;
		private readonly AppDbContext _db;
        public PaymentController(
			PaymentService paymentService,
			AppDbContext db, 
			BookingService bookingService,
			TravelPackageService travelPackageService,
			UserListService userListService)
        {
            _paymentService = paymentService;
			_bookingService = bookingService;
			_travelPackageService = travelPackageService;
			_userListService = userListService;

			_db = db;
        }




        // POST: Payment/Create
        [HttpPost]
        [ActionName("Save")]
        public async Task<IActionResult> SavePayment(PaymentRequestModel model)
        {
            if (ModelState.IsValid)
            {
                var response = await _paymentService.Execute(model);
                if (!response.IsSuccess)
                {
                    ModelState.AddModelError("", response.Message);
                    return RedirectToAction(nameof(Index));
                }

               
                TempData["PaymentSuccess"] = "Your payment was successful!";
            }
            return RedirectToAction("BookingHistory", "Home");
        }


    }
}

