﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;

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
       
        if (booking == null )
        {
            return new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Booking not found",
                Data = null
            };   
        }

        if (booking.Status != "Confirm")
        {
            return new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Payment can only be made for Confirmed bookings.",
                Data = null
            };
        }

       
        var payment = new Payment()
        {
            Id = Guid.NewGuid().ToString(),
            UserId = booking.UserId,
            BookingId = requestModel.BookingId,
            Amount = booking.TotalPrice,
            PaymentDate = DateTime.UtcNow,
            PaymentType = requestModel.paymentType,
            PaymentStatus = "Confirmed"
        };

        booking.Status = "Paid";
         _db.Bookings.Update(booking);
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
        var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == payment.BookingId);
        if (payment == null)
        {
            return new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Payment not found",
                Data = null
            };
        }
        payment.PaymentStatus = "Complete";
        booking.Status = "Success";
        _db.Bookings.Update(booking);
        _db.Payments.Update(payment);
        var result = await _db.SaveChangesAsync();
        return result == 2 ?
            new PaymentResponseModel
            {
                IsSuccess = true,
                Message = "Payment Completed successfully",
                Data = payment
            } :
            new PaymentResponseModel
            {
                IsSuccess = false,
                Message = "Payment Completed failed",
                Data = null
            };
    }
    public async Task<List<Payment>> GetPayments()
    {
        return await _db.Payments.AsNoTracking().ToListAsync();
    }

    public async Task<Payment> GetPaymentById(string id)
    {
        return (await _db.Payments.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id))!;
    }

    public async Task<List<PaymentData>> GetPaymentData()
    {
        var bookings = await _db.Bookings.AsNoTracking().ToListAsync();
        var users = await _db.Users.AsNoTracking().ToListAsync();
        var travelPackages = await _db.TravelPackages.AsNoTracking().ToListAsync();
        var payments = await _db.Payments.AsNoTracking().ToListAsync();
        var paymentData = new List<PaymentData>();
        foreach (var payment1 in payments)
        {
            var user = users.FirstOrDefault(u => u.Id == payment1.UserId);
            var booking = bookings.FirstOrDefault(b => b.Id == payment1.BookingId);
            var travelPackage = travelPackages.FirstOrDefault(t => t.Id == booking.TravelPackageId);
            if (travelPackage != null && user!=null && booking!=null) 
            {
                paymentData.Add(new PaymentData
                {
                    Id = payment1.Id,
                    User = user,
                    Booking = booking,
                    TravelPackage = travelPackage,
                    Amount = payment1.Amount,
                    PaymentDate = payment1.PaymentDate,
                    PaymentType = payment1.PaymentType,
                    PaymentStatus = payment1.PaymentStatus
                });
            }
        }
        return paymentData;
    }



    public async Task<List<PaymentData>> GetPaymentDataByUserId(string userId)
    {
        var payments = await GetPaymentData();
        return payments.Where(x => x.User.Id == userId).ToList();
    }
    public async Task<PaymentData?> GetPaymentDataByBookingId(string bookingId)
    {
        var payments = await GetPaymentData();


        return payments.FirstOrDefault(x => x.Booking.Id == bookingId);
    }

}




