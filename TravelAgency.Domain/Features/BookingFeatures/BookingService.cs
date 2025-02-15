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
        public async Task<BookingResponseModel> Execute(BookingRequestModel booking)
        {
            var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == booking.UserId);
            if (user == null)
            {
                return new BookingResponseModel { Success = false, Message = "User not found", Data = null };
            }
            var travelPackage = await _db.TravelPackages.AsNoTracking().FirstOrDefaultAsync(tp => tp.Id == booking.TravelPackageId);
            if (travelPackage == null)
            {
                return new BookingResponseModel { Success = false, Message = "Travel package not found", Data = null };
            }
            var Travelerlst = booking.Travelers;
            var Book = new Booking
            {
                Id = Guid.NewGuid().ToString(),
                UserId = booking.UserId,
                TravelPackageId = booking.TravelPackageId,
                NumberOfTravelers = Travelerlst.Count,
                TotalPrice = travelPackage.Price * Travelerlst.Count,
                BookingDate = DateTime.Now,
                TravelStartdate = booking.TravelStartdate,
                TravelEnddate = booking.TravelStartdate?.AddDays(travelPackage.Duration), // Fixed duration addition
                Status = "Pending"
            };

            _db.Bookings.Add(Book);
            var result = await _db.SaveChangesAsync();
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
            return result == 1 ?
                new BookingResponseModel { Success = true, Message = "Booking created successfully", Data = Book } :
                new BookingResponseModel { Success = false, Message = "Booking creation failed", Data = null };
        }

        public async Task<List<Booking>> GetBookings()
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
                    Status = booking.Status
                });
            }
            return bookingData;
        }
        public async Task<List<bookdata>> GetBookDatabyUserId(string userId)
        {
            var bookdata = await GetBookingData();
            return bookdata.Where(x => x.User.Id == userId).ToList();

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
            booking.Status = "Confirmed";
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
    }
}
