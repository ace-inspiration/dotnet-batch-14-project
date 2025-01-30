using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Domain.Features.AddTraveler;

[Route("api/bookings")]
[ApiController]
public class AddTravelerController : ControllerBase
{
	private AddTravelerService _service;
	public AddTravelerController(AddTravelerService addTravelerService)
	{
		_service = addTravelerService;
	}

	[HttpPost("{id}/add-traveler")]
	public async Task<IActionResult> Execute(
		string id,
		[FromBody] List<AddTravelerRequestModel> requestModels
		)
	{
		var response = await _service.Execute(id, requestModels);

		return response.Success ? StatusCode(201, response) : StatusCode(500, response);
	}
}
