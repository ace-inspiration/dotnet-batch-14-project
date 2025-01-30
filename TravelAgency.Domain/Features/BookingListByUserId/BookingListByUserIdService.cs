using Microsoft.EntityFrameworkCore;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.BookingListByUserId;

public class BookingListByUserIdService
{
	private readonly AppDbContext _db;
	public BookingListByUserIdService(AppDbContext db)
	{
		_db = db;
	}

	public async Task<BookingListByUserIdResponseModel> Execute(string userId)
	{
		BookingListByUserIdResponseModel responseModel = new();
		try
		{
			var bookings = await _db.Bookings.Where(x => x.UserId == userId).ToListAsync();
			responseModel.Success = true;
			responseModel.Message = "Operation successful.";
			responseModel.Data = bookings;
			return responseModel;
		}
		catch (Exception ex)
		{
			responseModel.Success = false;
			responseModel.Message = ex.ToString();
			return responseModel;
		}
	}
}
