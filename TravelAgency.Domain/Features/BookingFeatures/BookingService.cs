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

        public async Task<BookingResponseModel> RemoveTraveler(string bookingId, string travelerId)
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

        public async Task<BookingResponseModel> GetInvoice(string id)
        {
            BookingResponseModel model = new BookingResponseModel();
            var invoice = await _db.Bookings.Where(x => x.Id == id).FirstOrDefaultAsync();
            if(invoice is null)
            {
                model.Message = "Invoice not found!";
                return model;
            }

            model.Success = true;
            model.Message = "Success.";
            model.Data = invoice;

            return model;
        }
    }
}
