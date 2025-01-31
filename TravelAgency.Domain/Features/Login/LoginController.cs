using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Domain.Features.Login;

[Route("api/[controller]")]
[ApiController]
public class LoginController : ControllerBase
{
	private readonly LoginService _service;
	public LoginController(LoginService service)
	{
		_service = service;
	}

	[HttpPost]
	public async Task<IActionResult> Execute([FromBody] LoginRequestModel requestModel)
	{
		var response = await _service.Execute(requestModel);

		return response.Success ? Ok(response) : BadRequest(response);
	}

}
