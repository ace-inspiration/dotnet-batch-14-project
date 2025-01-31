using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.PaymentFeature
{
    public class PaymentService
    {
        private readonly AppDbContext _db;
        public PaymentService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<PaymentResponseModel> CreatePayment([FromBody] PaymentRequestModel requestModel)
        {

            PaymentResponseModel model = new PaymentResponseModel();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == requestModel.BookingId);


            if (booking == null)
            {
                model.Message = "Booking Id not found";

            }


            if (booking.Status != "Completed")
            {
                model.Message = "Payment can only be made for completed bookings.";

                return model;
            }

            if (requestModel.Amount <= 0 && requestModel.Amount != booking.TotalPrice)
            {
                model.Message = $"Payment amount must be exactly {booking.TotalPrice}.";
                return model;
            }

            var payment = new Payment()
            {
                Id = Guid.NewGuid().ToString(),
                BookingId = requestModel.BookingId,
                Amount = requestModel.Amount,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = requestModel.Status
            };

            await _db.Payments.AddAsync(payment);
            int result = await _db.SaveChangesAsync();

            string message = result > 0 ? "Payment Successfully" : "Payment Failed";
            model.Message = message;
            model.Data = payment;
            model.IsSuccess = result > 0;
            return model;
        }

    }
}
