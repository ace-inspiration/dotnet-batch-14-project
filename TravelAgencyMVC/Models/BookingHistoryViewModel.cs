using TravelAgency.Database.AppDbContextModels;

namespace TravelAgencyMVC.Models
{
    public class BookingHistoryViewModel
    {
        public IEnumerable<Booking> Bookings { get; set; }
        public IEnumerable<TravelPackage> Packages { get; set; }

        
    }
}
