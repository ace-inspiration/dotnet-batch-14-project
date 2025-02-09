using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.ActivateTravelPackage;
using TravelAgency.Domain.Features.AddTraveler;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.BookingListByUserId;
using TravelAgency.Domain.Features.DeactivateTravelPackage;
using TravelAgency.Domain.Features.Login;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.PaymentListByUserId;
using TravelAgency.Domain.Features.TravelersListByBookingId;
using TravelAgency.Domain.Features.TravelPackage;
using TravelAgency.Domain.Features.UserLists;
using TravelAgency.Domain.Features.UserRegister;

namespace TravelAgency.Domain;

public static class FeatureManager
{
    public static void AddTravelAgencyFeatures(this WebApplicationBuilder builder)
    {
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            options.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
            options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnection"));
            //options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnectionpkk"));
        },
        ServiceLifetime.Transient,
        ServiceLifetime.Transient);


        builder.Services.AddScoped<AddTravelerService>();
        builder.Services.AddScoped<BookingListByUserIdService>();

        builder.Services.AddScoped<BookingService>();
        builder.Services.AddScoped<DeactivateTravelPackageService>();
        builder.Services.AddScoped<ActivateTravelPackageService>();

        builder.Services.AddScoped<UserRegisterService>();
        builder.Services.AddScoped<PaymentService>();

        builder.Services.AddScoped<LoginService>();

        builder.Services.AddScoped<TravelPackageService>();

        builder.Services.AddScoped<TravelersListService>();

        builder.Services.AddScoped<UserListService>();
        builder.Services.AddScoped<PaymentListByUserIdService>();
    }
}
