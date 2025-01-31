using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.BookingFeatures;
[Route("api/[controller]")]
[ApiController]
public class BookingsController : ControllerBase
{
    private BookingService _booking;
    public BookingsController(BookingService bookingService)
    {
        _booking = bookingService;
    }
    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingRequestModel requestModel        )
    {
        var response = await _booking.CreateBooking(requestModel);
        BookingResponseModel responseModel = new BookingResponseModel 
        {
            Success = response.Success,
            Message = response.Message,
            Data = response.Data
        };
        return response.Success ? StatusCode(201, responseModel) : StatusCode(500, responseModel);
    }

    [HttpGet]
    public async Task<IActionResult> GetBookings()
    {
        var response = await _booking.GetBookings();
        return response.Count > 0 ? StatusCode(200, response) : StatusCode(404, response);
    }

}
