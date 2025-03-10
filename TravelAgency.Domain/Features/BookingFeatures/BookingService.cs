﻿using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.PaymentFeature;

namespace TravelAgency.Domain.Features.BookingFeatures;

public class BookingService
{
    private readonly AppDbContext _db;
    public BookingService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<BookingResponseModel> Execute(BookingRequestModel booking)
    {
        // Check if the user exists
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == booking.UserId);
        if (user == null)
        {
            return new BookingResponseModel { Success = false, Message = "User not found", Data = null };
        }

        // Check if the travel package exists
        var travelPackage = await _db.TravelPackages.AsNoTracking().FirstOrDefaultAsync(tp => tp.Id == booking.TravelPackageId);
        if (travelPackage == null)
        {
            return new BookingResponseModel { Success = false, Message = "Travel package not found", Data = null };
        }

        // Generate the invoice number
        var lastBooking = await _db.Bookings
            .OrderByDescending(b => b.InvoiceNumber)
            .FirstOrDefaultAsync();

        int lastInvoiceNumber = 0;
        if (lastBooking != null && int.TryParse(lastBooking.InvoiceNumber, out int parsedNumber))
        {
            lastInvoiceNumber = parsedNumber;
        }
        string invoiceNumber = (lastInvoiceNumber + 1).ToString("D3"); // Format as 3 digits (e.g., 001, 002, etc.)
        int tripDuration = (booking.TravelEnddate.Value - booking.TravelStartdate.Value).Days;
        decimal totalPrice = tripDuration * travelPackage.Price * booking.Travelers.Count;
        // Create the booking
        var Travelerlst = booking.Travelers;
        var Book = new Booking
        {
            Id = Guid.NewGuid().ToString(),
            UserId = booking.UserId,
            TravelPackageId = booking.TravelPackageId,
            NumberOfTravelers = Travelerlst.Count,
            TotalPrice =totalPrice,
            BookingDate = DateTime.Now,
            TravelStartdate = booking.TravelStartdate,
            TravelEnddate = booking.TravelEnddate,
            //TravelEnddate = booking.TravelStartdate?.AddDays(travelPackage.Duration),
            InvoiceNumber = invoiceNumber, // Assign the generated invoice number
            Status = "Primary"

        };

        // Add the booking to the database
        _db.Bookings.Add(Book);

        travelPackage.Count++;
        _db.TravelPackages.Update(travelPackage);
        var result = await _db.SaveChangesAsync();

        // Add travelers to the database
        foreach (var traveler in Travelerlst)
        {
            var Traveler = new Traveler
            {
                Id = Guid.NewGuid().ToString(),
                BookingId = Book.Id,
                Name = traveler.Name,
                Age = traveler.Age,
                Gender = traveler.Gender
            };
            _db.Travelers.Add(Traveler);
            var res = await _db.SaveChangesAsync();
            if (res != 1)
            {
                return new BookingResponseModel { Success = false, Message = "Traveler addition failed", Data = null };
            }
        }

        // Return the response
        return result == 2 ?
            new BookingResponseModel { Success = true, Message = "Booking created successfully", Data = Book } :
            new BookingResponseModel { Success = false, Message = "Booking creation failed", Data = null };
    }

    public async Task<List<Booking>> Execute()
    {
        return await _db.Bookings.ToListAsync();
    }

    public async Task<List<bookdata>> GetBookingData()
    {
        var bookings = await _db.Bookings.AsNoTracking().ToListAsync();
        var users = await _db.Users.AsNoTracking().ToListAsync();
        var travelPackages = await _db.TravelPackages.AsNoTracking().ToListAsync();
        var bookingData = new List<bookdata>();
        foreach (var booking in bookings)
        {
            var user = users.FirstOrDefault(user => user.Id == booking.UserId);
            var travelPackage = travelPackages.FirstOrDefault(package => package.Id == booking.TravelPackageId);
            bookingData.Add(new bookdata
            {
                Id = booking.Id,
                User = user,
                TravelPackage = travelPackage,
                NumberOfTravelers = booking.NumberOfTravelers,
                TotalPrice = booking.TotalPrice,
                BookingDate = booking.BookingDate,
                TravelStartdate = booking.TravelStartdate,
                TravelEnddate = booking.TravelEnddate,
                Status = booking.Status,
                InvoiceNumber = booking.InvoiceNumber
            });
        }
        return bookingData;
    }
    public async Task<List<bookdata>> GetBookingDataByUserId(string userId)
    {
        var bookdata = await GetBookingData();
        return bookdata.Where(x => x.User.Id == userId).ToList();
    }


    public async Task<bookdata?> GetBookingDataByBookingId(string bookingId)
    {
        var books = await GetBookingData();


        return books.FirstOrDefault(x => x.Id == bookingId);
    }


    public async Task<BookingResponseModel> Execute(string bookingId, string travelerId)
    {
        BookingResponseModel model = new BookingResponseModel();
        var booking = await _db.Bookings.Where(x => x.Id == bookingId).FirstOrDefaultAsync();
        if (booking is null)
        {
            model.Message = "Booking not found!";
            return model;
        }

        var traveler = await _db.Travelers.Where(x => x.Id == travelerId).FirstOrDefaultAsync();
        if (traveler is null)
        {
            model.Message = "Traveler not found!";
            return model;
        }

        if (booking.Id == traveler.Id)
        {
            model.Message = "Traveler is not found in this Booking Package!";
            return model;
        }

        booking.NumberOfTravelers -= 1;
        var travelPackage = await _db.TravelPackages.AsNoTracking().FirstOrDefaultAsync(tp => tp.Id == booking.TravelPackageId);
        booking.TotalPrice -= travelPackage.Price;

        _db.Travelers.Remove(traveler);
        _db.Bookings.Update(booking);

        var result = await _db.SaveChangesAsync();

        return new BookingResponseModel
        {
            Success = result > 0,
            Message = result > 0 ? "Successfully remove Traveler from Booking." : "Traveler remove from Booking failed.",
            Data = booking
        };
    }

    public async Task<BookingResponseModel> Execute(string id)
    {
        BookingResponseModel model = new BookingResponseModel();
        var invoice = await _db.Bookings.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (invoice is null)
        {
            model.Message = "Invoice not found!";
            return model;
        }

        model.Success = true;
        model.Message = "Success.";
        model.Data = invoice;
        return model;
    }

    public async Task<BookingResponseModel> ConfirmBooking(string id)
    {
        BookingResponseModel model = new BookingResponseModel();
        var booking = await _db.Bookings.Where(x => x.Id == id).FirstOrDefaultAsync();
        if (booking is null)
        {
            model.Message = "Booking not found!";
            return model;
        }
        booking.Status = "Confirm";
        _db.Bookings.Update(booking);
        var result = await _db.SaveChangesAsync();
        return new BookingResponseModel
        {
            Success = result > 0,
            Message = result > 0 ? "Booking confirmed successfully." : "Booking confirmation failed.",
            Data = booking
        };
    }

    internal async Task GetBookingdata()
    {
        throw new NotImplementedException();
    }


    public async Task<BookingResponseModel> DeleteBookingHistory (string bookingId)
    {
        var item = _db.Bookings.AsNoTracking().FirstOrDefault(x => x.Id == bookingId);
        var result =  _db.Entry(item).State = EntityState.Deleted;
        _db.SaveChanges();
        return new BookingResponseModel 
        { Success = result > 0,
            Message = "Deleting successful", 
            Data = null };
    }

    public async Task<HomeModel> GetHomeData()
    {
        var today = DateTime.Today;
        var lastSunday = today.AddDays(-(int)today.DayOfWeek); // Ensure start from Sunday
        var last7Days = Enumerable.Range(0, 7).Select(i => lastSunday.AddDays(i)).ToList();

        // Fetch Booking Counts (Group by Date)
        var bookingCounts = await _db.Bookings
            .Where(b => b.BookingDate >= last7Days.First() && b.BookingDate <= last7Days.Last())
            .GroupBy(b => b.BookingDate.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Date, g => g.Count);

        // Fetch Payment Counts (Group by Date)
        var paymentCounts = await _db.Payments
            .Where(p => p.PaymentDate >= last7Days.First() && p.PaymentDate <= last7Days.Last())
            .GroupBy(p => p.PaymentDate.Date)
            .Select(g => new { Date = g.Key, Count = g.Count() })
            .ToDictionaryAsync(g => g.Date, g => g.Count);

        var homeData = new HomeModel
        {
            countallbooking = await _db.Bookings.CountAsync(),
            countallpayment = await _db.Payments.CountAsync(),
            countalltraveler = await _db.Travelers.CountAsync(),
            countalluser = await _db.Users.CountAsync(),
            countalltravelPackage = await _db.TravelPackages.CountAsync(),
            countverifybooking = await _db.Bookings.CountAsync(b => b.Status == "Primary"),
            countverifiedbooking = await _db.Bookings.CountAsync(b => b.Status == "Success"),
            countverifypayment = await _db.Payments.CountAsync(p => p.PaymentStatus == "Confirmed"),
            countverifiedpayment = await _db.Payments.CountAsync(p => p.PaymentStatus == "Complete"),
            countcompletepayment = await _db.Payments.CountAsync(p => p.PaymentStatus == "Complete"),
            counttodaybookingPackage = await _db.Bookings.CountAsync(b => b.BookingDate.Date == today),
            counttodayBooking = await _db.Bookings.CountAsync(b => b.BookingDate.Date == today),
            counttodayPayment = await _db.Payments.CountAsync(p => p.PaymentDate.Date == today),

            // Ensure Data is Ordered from Sunday to Saturday
            bookingweekdata = last7Days.Select(date => bookingCounts.ContainsKey(date) ? bookingCounts[date] : 0).ToArray(),
            paymentweekdata = last7Days.Select(date => paymentCounts.ContainsKey(date) ? paymentCounts[date] : 0).ToArray()
        };

        return homeData;
    }

}
