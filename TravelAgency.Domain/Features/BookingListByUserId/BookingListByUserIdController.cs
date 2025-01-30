using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Domain.Features.BookingListByUserId;

[Route("api/users")]
[ApiController]
public class BookingListByUserIdController : ControllerBase
{
	private readonly BookingListByUserIdService _service;
	public BookingListByUserIdController(BookingListByUserIdService service)
	{
		_service = service;
	}

	[HttpGet("{id}/bookings")]
	public async Task<IActionResult> Execute(string id)
	{
		var response = await _service.Execute(id);

		return response.Success ? Ok(response) : StatusCode(500, response);
	}
}
