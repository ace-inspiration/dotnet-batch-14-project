using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Domain.Features.ActivateTravelPackage;

[Route("api/Admin/[controller]")]
[ApiController]

public class ActivateTravelPackageController : ControllerBase
{
    private readonly ActivateTravelPackageService _service;
    public ActivateTravelPackageController(ActivateTravelPackageService activateTravelPackageService)
    {
        _service = activateTravelPackageService;
    }

    [HttpPost("{id}/activate")]
    public async Task<IActionResult> Execute(
        string id
        )
    {
        var response = await _service.Execute(id);

        return response.Success ? StatusCode(201, response) : StatusCode(500, response);
    }
}


