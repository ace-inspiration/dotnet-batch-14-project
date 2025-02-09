using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravelAgency.Domain.Features.TravelPackages;

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
            var response = await _travelPackageService.Execute();

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

        [HttpPost("travel-packages")]
        public async Task<IActionResult> AddTravelPackage([FromForm] TravelPackageRequestModel travelPackage, IFormFile? photo)
        {
            try
            {
                var response = await _travelPackageService.CreateTravelPackage(travelPackage,photo);
                return response.Success == true ? Ok(response) : BadRequest(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Success = "false", Message = ex.Message });
            }
        }
    }
}
