using Microsoft.AspNetCore.Http.HttpResults;
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
        var response = await _booking.Execute(requestModel);
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

    [HttpPost("{id}/remove-traveler/{traveler_id}")]
    public async Task<IActionResult> RemoveTraveler(string id, string traveler_id)
    {
        try
        {
            var response = await _booking.Execute(id, traveler_id);
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

    [HttpGet("{id}/invoice")]
    public async Task<IActionResult> GetInvoice(string id)
    {
        try
        {
            var response = await _booking.Execute(id);
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

    [HttpPost("{id}/confirm")]
    public async Task<IActionResult> ConfirmBooking(string id)
    {
        try
        {
            var response = await _booking.ConfirmBooking(id);
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


    [HttpGet("GetBookingData")]
    public async Task<IActionResult> GetBookingData()
    {
        try
        {
            var bookingData = await _booking.GetBookingData();
            if (bookingData == null)
            {
                return NotFound("Booking data not found.");
            }
            return Ok(bookingData);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

}
