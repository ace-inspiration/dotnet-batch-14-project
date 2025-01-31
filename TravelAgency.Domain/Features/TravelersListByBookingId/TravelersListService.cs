using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;

namespace TravelAgency.Domain.Features.TravelersListByBookingId
{
    public class TravelersListService
    {
        private readonly AppDbContext _db;
        public TravelersListService(AppDbContext db)
        {
            _db = db;
        }
        public async Task<BookingResponseModel> GetTravelersByBookingIdAsync(string bookingId)
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

    }
}
