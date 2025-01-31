using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.BookingFeatures;

public class BookingController : ControllerBase
{
    private BookingService _booking;
    public BookingController(BookingService bookingService)
    {
        _booking = bookingService;
    }
    [HttpPost("create-booking")]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequestModel requestModel        )
    {
        var response = await _booking.CreateBooking(requestModel);
        return response.Success ? StatusCode(201, response) : StatusCode(500, response);
    }

    [HttpGet("get-booking")]
    public async Task<IActionResult> GetBooking()
    {
        var response = await _booking.GetBookings();
        return response.Count > 0 ? StatusCode(200, response) : StatusCode(404, response);
    }

}
