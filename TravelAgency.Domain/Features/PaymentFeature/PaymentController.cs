using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Features.UserRegister;

namespace TravelAgency.Domain.Features.PaymentFeature
{
    [Route("api/payment")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }


        [HttpPost("create-payment")]
        public async Task<IActionResult> CreatePayment([FromBody] PaymentRequestModel requestModel)
        {
            try
            {
                var payment = await _paymentService.CreatePayment(requestModel);
                if (!payment.IsSuccess)
                {
                    return BadRequest(payment);
                }
                return Ok(payment);
            }
            catch (Exception ex)
            {
                return BadRequest(new PaymentResponseModel()
                {
                    Message = ex.ToString()
                });
            }
        }

    }
}
