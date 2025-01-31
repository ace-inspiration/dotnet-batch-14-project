using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.BookingFeatures
{
    public class BookingService
    {
        private readonly AppDbContext _db;
        public BookingService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<BookingResponseModel> CreateBooking(BookingRequestModel requestModel)
        {
            BookingResponseModel response = new();
            try
            {
                // Check if User exists
                var userExists = await _db.Users.AnyAsync(u => u.Id == requestModel.UserId);
                if (!userExists)
                {
                    return new BookingResponseModel
                    {
                        Success = false,
                        Message = "User not found!",
                        Data = null!
                    };
                }

                // Check if Travel Package exists
                var travelPackage = await _db.TravelPackages.FirstOrDefaultAsync(tp => tp.Id == requestModel.TravelPackageId);
                if (travelPackage == null)
                {
                    return new BookingResponseModel
                    {
                        Success = false,
                        Message = "Travel package not found!",
                        Data = null!
                    };
                }

                var booking = new Booking
                {
                    Id = Guid.NewGuid().ToString(),
                    UserId = requestModel.UserId,
                    TravelPackageId = requestModel.TravelPackageId,
                    NumberOfTravelers = requestModel.NumberOfTravelers,
                    TotalPrice = travelPackage.Price * requestModel.NumberOfTravelers,
                    BookingDate = DateTime.Now,
                    Status = "Comfirmed"
                };

                // Save booking
                await _db.Bookings.AddAsync(booking);
                var result = await _db.SaveChangesAsync();

                return new BookingResponseModel
                {
                    Success = result > 0,
                    Message = result > 0 ? "Booking created successfully." : "Booking creation failed.",
                    Data = booking
                };
            }
            catch (Exception ex)
            {
                response.Success = false;
                response.Message = ex.ToString();
                return response;
            }
        }

        public async Task<List<Booking>> GetBookings()
        {
            return await _db.Bookings.ToListAsync();
        }
    }
}
