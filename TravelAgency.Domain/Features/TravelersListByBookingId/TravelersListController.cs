using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;
using TravelAgency.Domain.Features.BookingFeatures;

namespace TravelAgency.Domain.Features.TravelersListByBookingId
{
    [Route("api/[controller]")]
    [ApiController]
    public class TravelersListController : ControllerBase
    {
        private TravelersListService _traveler;
        public TravelersListController(TravelersListService travelersListService)
        {
            _traveler = travelersListService;
        }
        [HttpGet("{id}/travelers")]
        public async Task<IActionResult> GetTravelersByBookingId(string id)
        {
            try
            {
                var response = await _traveler.Execute(id);
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BookingResponseModel
                {
                    Message = ex.Message,
                });
            }
        }

        [HttpGet("travelerdata")]
        public async Task<IActionResult> GetTravelerdata()
        {
            try
            {
                var response = await _traveler.Travelerdatas();
                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new BookingResponseModel
                {
                    Message = ex.Message,
                });
            }
        }

    }
}
