using TravelAgency.Database.AppDbContextModels;

namespace TravelAgencyMVC.Models
{
    public class BookingHistoryViewModel
    {
        public List<Booking> Bookings { get; set; }
        public List<TravelPackage> Packages { get; set; }

        
    }
}
