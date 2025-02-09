using Azure.Identity;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.TravelPackages;

namespace TravelAgencyMVC.Models;

public class AdminDashboardViewModel
{
    public List<bookdata> Bookings { get; set; }
    public List<PaymentData> Payments { get; set; }
    public List<Traveler> Travelers { get; set; }
    public List<User> Users { get; set; }
    public List<TravelPackageRequestModel> TravelPackages { get; set; }
    public List<Booking> ConfirmedBookings { get; set; }
    public List<Payment> ConfirmedPayments { get; set; }
}



