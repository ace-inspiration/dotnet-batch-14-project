using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using TravelAgency.Domain.Features.AddTraveler;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.BookingListByUserId;
using TravelAgency.Domain.Features.Login;
using TravelAgency.Domain.Features.PaymentFeature;
using TravelAgency.Domain.Features.PaymentListByUserId;
using TravelAgency.Domain.Features.TravelersListByBookingId;
using TravelAgency.Domain.Features.TravelPackage;
using TravelAgency.Domain.Features.UserLists;
using TravelAgency.Domain.Features.UserRegister;

namespace TravelAgency.Domain.Features;

public static class FeatureServiceExtension
{
	public static void AddFeatureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<AddTravelerService>();
		builder.Services.AddScoped<BookingListByUserIdService>();

		builder.Services.AddScoped<BookingService>();

		builder.Services.AddScoped<UserRegisterService>();
		builder.Services.AddScoped<PaymentService>();

		builder.Services.AddScoped<LoginService>();

		builder.Services.AddScoped<TravelPackageService>();

		builder.Services.AddScoped<TravelersListService>();

        builder.Services.AddScoped<UserListService>();
        builder.Services.AddScoped<PaymentListByUserIdService>();
    }
}
