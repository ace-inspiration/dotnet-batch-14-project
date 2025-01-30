using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.AddTraveler;

public class AddTravelerService
{
	private readonly AppDbContext _db;
	public AddTravelerService(AppDbContext db)
	{
		_db = db;
	} 

	public async Task<AddTravelerResponseModel> Execute(string bookingId, List<AddTravelerRequestModel> requestModel)
	{
		AddTravelerResponseModel response = new();
		try
		{
			var booking = await _db.Bookings.FirstOrDefaultAsync(x => x.Id == bookingId);
			if(booking is null)
			{
				response.Success = false;
				response.Message = "Booking not found.";
				return response;
			}

			var travelers = requestModel.Select(x =>
			{
				return new Traveler
				{
					Id = Guid.NewGuid().ToString(),
					BookingId = bookingId,
					Name = x.Name,
					Age = x.Age,
					Gender = x.Gender
				};
			}).ToList();

			await _db.Travelers.AddRangeAsync(travelers);
			await _db.SaveChangesAsync();

			response.Success = true;
			response.Message = "Operation successful.";
			response.Data = travelers;
			return response;
		}
		catch (Exception ex)
		{
			response.Success = false;
			response.Message = ex.ToString();
			return response;
		}
	}
}
