using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Features.BookingListByUserId;

namespace TravelAgency.Domain.Features.PaymentListByUserId
{
    [Route("api/users")]
    [ApiController]
    public class PaymentListByUserIdController : ControllerBase
    {
        private readonly PaymentListByUserIdService _service;
        public PaymentListByUserIdController(PaymentListByUserIdService service)
        {
            _service = service;
        }

        [HttpGet("{id}/payments")]
        public async Task<IActionResult> Execute(string id)
        {
            var response = await _service.Execute(id);

            return response.Success ? Ok(response) : StatusCode(500, response);
        }
    }
}
