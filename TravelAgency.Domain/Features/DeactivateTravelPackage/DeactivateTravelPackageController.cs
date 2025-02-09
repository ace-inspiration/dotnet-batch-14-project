using Microsoft.AspNetCore.Mvc;

namespace TravelAgency.Domain.Features.DeactivateTravelPackage;

[Route("api/Admin/[controller]")]
[ApiController]

public class DeactivateTravelPackageController : ControllerBase
{
    private readonly DeactivateTravelPackageService _service;
    public DeactivateTravelPackageController(DeactivateTravelPackageService deactivateTravelPackageService)
    {
        _service = deactivateTravelPackageService;
    }

    [HttpPost("deactivate")]
    public async Task<IActionResult> Execute(string id)
    {
        var response = await _service.Execute(id);

        return response.Success ? StatusCode(201, response) : StatusCode(500, response);
    }
}