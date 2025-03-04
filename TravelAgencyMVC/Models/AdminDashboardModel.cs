using Azure.Identity;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.TravelersListByBookingId;
using TravelAgency.Domain.Features.TravelPackages;

namespace TravelAgencyMVC.Models;

public class AdminDashboardViewModel
{
    public List<bookdata> Bookings { get; set; }
    public List<PaymentData> Payments { get; set; }
    public List<Travelerdata> Travelers { get; set; }
    public List<User> Users { get; set; }
    public List<TravelPackage> TravelPackages { get; set; }
    public HomeModel ?homeModel { get; set; }
}




