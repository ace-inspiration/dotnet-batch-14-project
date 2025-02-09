using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.PaymentFeature;

public class PaymentService
{
    private readonly AppDbContext _db;
    public PaymentService(AppDbContext db)
    {
        _db = db;
    }    

    public async Task<PaymentResponseModel> Execute(PaymentRequestModel requestModel)
    {           

        var booking = await _db.Bookings.AsNoTracking().FirstOrDefaultAsync(b => b.Id == requestModel.BookingId);

        if (booking == null)
        {
            return new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Booking not found",
                Data = null
            };   
        }

        if (booking.Status != "Confirmed")
        {
            return new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Payment can only be made for Confirmed bookings.",
                Data = null
            };
        }

        if (requestModel.Amount != booking.TotalPrice)
        {
            return new PaymentResponseModel
            {
                IsSuccess = false,
                Message = $"Payment amount must be exactly {booking.TotalPrice}."
            };
        }

        var payment = new Payment()
        {
            Id = Guid.NewGuid().ToString(),
            UserId = requestModel.UserId,
            BookingId = requestModel.BookingId,
            Amount = requestModel.Amount,
            PaymentDate = DateTime.UtcNow,
            PaymentStatus = "Pending"
        };

        await _db.Payments.AddAsync(payment);
        var result = await _db.SaveChangesAsync();
        return result == 2 ?
            new PaymentResponseModel
            {
                IsSuccess = true,
                Message = "Payment created successfully",
                Data = payment
            } :
            new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Payment creation failed",
                Data = null
            };
    }
    public async Task<PaymentResponseModel> ConfirmPayment(string paymentId)
    {
        var payment = await _db.Payments.FirstOrDefaultAsync(p => p.Id == paymentId);
        if (payment == null)
        {
            return new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Payment not found",
                Data = null
            };
        }
        payment.PaymentStatus = "Confirmed";
        _db.Payments.Update(payment);
        var result = await _db.SaveChangesAsync();
        return result == 1 ?
            new PaymentResponseModel
            {
                IsSuccess = true,
                Message = "Payment confirmed successfully",
                Data = payment
            } :
            new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Payment confirmation failed",
                Data = null
            };
    }


    public async Task<List<Payment>> GetPayments()
    {
        return await _db.Payments.AsNoTracking().ToListAsync();
    }

    public async Task<Payment> GetPaymentById(string id)
    {
        return await _db.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);
    }



}




