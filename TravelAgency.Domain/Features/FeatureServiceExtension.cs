using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Features.AddTraveler;
using TravelAgency.Domain.Features.BookingFeatures;
using TravelAgency.Domain.Features.BookingListByUserId;

namespace TravelAgency.Domain.Features;

public static class FeatureServiceExtension
{
	public static void AddFeatureServices(this WebApplicationBuilder builder)
	{
		builder.Services.AddScoped<AddTravelerService>();
		builder.Services.AddScoped<BookingListByUserIdService>();
		builder.Services.AddScoped<BookingService>();
    }
}
