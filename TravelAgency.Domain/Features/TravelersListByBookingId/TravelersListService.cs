using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;

namespace TravelAgency.Domain.Features.TravelersListByBookingId;

public class TravelersListService
{
    private readonly AppDbContext _db;
    public TravelersListService(AppDbContext db)
    {
        _db = db;
    }
    public async Task<BookingResponseModel> Execute(string bookingId)
    {
        var travelers = await _db.Travelers
            .Where(t => t.BookingId == bookingId)
            .ToListAsync();

        if (!travelers.Any())
        {
            return new BookingResponseModel
            {
                Success = false,
                Message = "No travelers found for this booking.",
                Data = null!
            };
        }

        return new BookingResponseModel
        {
            Success = true,
            Message = "Travelers retrieved successfully.",
            Data = travelers
        };
    }
    public async Task<List<Traveler>> GetTravelers()
    {
        return await _db.Travelers.AsNoTracking().ToListAsync();
    }
    public async Task<List<Travelerdata>> Travelerdatas()
    {
        var bookings = await _db.Bookings.ToListAsync();
        var travelPackages = await _db.TravelPackages.ToListAsync();
        var travelers = await _db.Travelers.ToListAsync();
        var Travelerdata = new List<Travelerdata>();
        foreach (var booking in bookings)
        {
            var travelpackage = travelPackages.FirstOrDefault(tp => tp.Id == booking.TravelPackageId);
            var traveler = travelers.Where(t => t.BookingId == booking.Id).ToList();
            foreach (var t in traveler)
            {
                Travelerdata.Add(new Travelerdata
                {
                    booking = booking,
                    TravelPackage = travelpackage,
                    Name = t.Name,
                    Age = t.Age,
                    Gender = t.Gender,
                });
            }
        }
        return Travelerdata;
    }

}
