﻿using Microsoft.AspNetCore.Mvc;
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

        //public async Task<PaymentResponseModel> CreatePayment(PaymentRequestModel requestModel)
        //{

        //    PaymentResponseModel model = new PaymentResponseModel();

        //    var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == requestModel.BookingId);


        //    if (booking == null)
        //    {
        //        model.Message = "Booking Id not found";

        //    }


        //    if (booking.Status != "Confirmed")
        //    {

        //        model.Message = "Payment can only be made for Confirmed bookings.";

        //        return model;
        //    }

        //    if (requestModel.Amount != booking.TotalPrice)
        //    {
        //        model.Message = $"Payment amount must be exactly {booking.TotalPrice}.";
        //        return model;
        //    }

        //    var payment = new Payment()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        BookingId = requestModel.BookingId,
        //        Amount = requestModel.Amount,
        //        PaymentDate = DateTime.UtcNow,
        //        PaymentStatus = "Pending"
        //    };

        //    await _db.Payments.AddAsync(payment);
        //    await _db.SaveChangesAsync();

        //    bool isPaymentSuccessful = SimulatePaymentProcessing();

        //    if (isPaymentSuccessful)
        //    {
        //        payment.PaymentStatus = "Completed";
        //        booking.Status = "Completed";
        //        _db.Bookings.Update(booking);

        //        await _db.SaveChangesAsync();
        //        //string message = result > 0 ? "Payment successful, booking completed." : "Payment Failed";
        //        model.IsSuccess = true;
        //        model.Message = "Payment successful, booking completed.";
        //        model.Data = payment;

        //        return model;
        //    }
        //    else
        //    {
        //        payment.PaymentStatus = "Failed";
        //        await _db.SaveChangesAsync();
        //        model.IsSuccess = false;
        //       model.Message = "Payment failed. Please try again.";
        //        model.Data = payment;

        //        return model;
        //    }

        //}

        //private bool SimulatePaymentProcessing()
        //{
        //    var random = new Random().Next(0, 2) == 1;
        //    return random;
        //}



        //public async Task<PaymentResponseModel> CreatePayment(PaymentRequestModel requestModel)
        //{
        //    PaymentResponseModel model = new PaymentResponseModel();

        //    var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == requestModel.BookingId);
        //    if (booking == null)
        //    {
        //        model.Message = "Booking Id not found.";
        //        return model;
        //    }

        //    if (booking.Status != "Confirmed")
        //    {
        //        model.Message = "Payment can only be made for Confirmed bookings.";
        //        return model;
        //    }

        //    if (requestModel.Amount != booking.TotalPrice)
        //    {
        //        model.Message = $"Payment amount must be exactly {booking.TotalPrice}.";
        //        return model;
        //    }

        //    var payment = new Payment()
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        BookingId = requestModel.BookingId,
        //        Amount = requestModel.Amount,
        //        PaymentDate = DateTime.UtcNow,
        //        PaymentStatus = "Completed"
        //    };

        //    await _db.Payments.AddAsync(payment);
        //   int result= await _db.SaveChangesAsync();


        //    if (model.IsSuccess = result > 0)
        //    {
        //        model.IsSuccess = true;
        //        model.Message = "Payment successful, booking completed.";
        //        model.Data = payment;
        //        booking.Status = "Completed";

        //    }
        //    else
        //    {
        //        model.IsSuccess = false;
        //        model.Message = "Payment failed. Please try again.";
        //        model.Data = payment;
        //        booking.Status = "Failed";
        //    }


        //     _db.Bookings.Update(booking);
        //    await _db.SaveChangesAsync();

        //    return model;
        //}


        public async Task<PaymentResponseModel> CreatePayment(PaymentRequestModel requestModel)
        {
            PaymentResponseModel model = new PaymentResponseModel();

            var booking = await _db.Bookings.FirstOrDefaultAsync(b => b.Id == requestModel.BookingId);

            if (booking == null)
            {
                model.Message = "Booking ID not found.";
                return model;
            }

            if (booking.Status != "Confirmed")
            {
                model.Message = "Payment can only be made for Confirmed bookings.";
                return model;
            }

            if (requestModel.Amount != booking.TotalPrice)
            {
                model.Message = $"Payment amount must be exactly {booking.TotalPrice}.";
                return model;
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
            await _db.SaveChangesAsync();

            // Process Payment
            bool isPaymentSuccessful = ProcessPayment(payment.Id);

            if (isPaymentSuccessful)
            {
                payment.PaymentStatus = "Completed";
                booking.Status = "Completed";
                _db.Bookings.Update(booking);
                await _db.SaveChangesAsync();

                model.IsSuccess = true;
                model.Message = "Payment successful, booking completed.";
                model.Data = payment;
            }
            else
            {
                payment.PaymentStatus = "Failed";
                await _db.SaveChangesAsync();

                model.IsSuccess = false;
                model.Message = "Payment failed. Please try again.";
                model.Data = payment;
            }

            return model;
        }

        public bool ProcessPayment(string paymentId)
        {
            var payment = _db.Payments.FirstOrDefault(p => p.Id == paymentId);

            if (payment == null || payment.PaymentStatus != "Pending")
                return false;      // Payment cannot be processed

            payment.PaymentStatus = "Completed";    
            _db.SaveChanges();

            return true;
        }

       
    }
}

      
    

