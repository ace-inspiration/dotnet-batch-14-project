using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelAgency.Domain.Features.TravelPackage;

namespace TravelAgency.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelPackageController : ControllerBase
    {
        private readonly TravelPackageService _travelPackageService;

        // Constructor with Dependency Injection
        public TravelPackageController(TravelPackageService travelPackageService)
        {
            _travelPackageService = travelPackageService;
        }

        // GET: /api/travel-packages
        [HttpGet]
        public async Task<IActionResult> GetTravelPackages()
        {
            var response = await _travelPackageService.GetAllTravelPackagesAsync();

            if (response == null || response.Count == 0)
            {
                return NotFound(new TravelPackageResponseModel
                {
                    Success = false,
                    Message = "No travel packages found.",
                    Data = null
                });
            }

            return Ok(new TravelPackageResponseModel
            {
                Success = true,
                Message = "Travel packages retrieved successfully.",
                Data = response
            });
        }
    }
}
