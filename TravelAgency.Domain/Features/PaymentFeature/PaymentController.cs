using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Domain.Features.UserRegister;

namespace TravelAgency.Domain.Features.PaymentFeature;

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
    public async Task<IActionResult> Execute([FromBody] PaymentRequestModel requestModel)
    {
        try
        {
            var payment = await _paymentService.Execute(requestModel);
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
    [HttpPost("confirm-payment")]
    public async Task<IActionResult> ConfirmPayment(string id)
    {
        try
        {
            var payment = await _paymentService.ConfirmPayment(id);
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

    [HttpGet("GetPayments")]
    public async Task<IActionResult> GetPayments()
    {
        try
        {
            var payments = await _paymentService.GetPayments();

            return Ok(new PaymentListResponseModel
            {
                IsSuccess = true,
                Message = "Success",
                Data = payments
            });
        }
        catch (Exception ex)
        {
            return BadRequest(new PaymentResponseModel
            {
                IsSuccess = false,
                Message = ex.ToString(),
                Data = null
            });
        }
    }

    [HttpGet("GetPayment/{id}")]
    public async Task<IActionResult> GetPaymentById(string id)
    {
        try
        {
            var payment = await _paymentService.GetPaymentById(id);

            if (payment == null)
            {
                return NotFound(new PaymentResponseModel
                {
                    IsSuccess = false,
                    Message = "Payment not found",
                    Data = null
                });
            }

            return Ok(new PaymentResponseModel
            {
                IsSuccess = true,
                Message = "Success",
                Data = payment
            });

        }
        catch (Exception ex)
        {
            return BadRequest(new PaymentResponseModel
            {
                IsSuccess = false,
                Message = ex.ToString(),
                Data = null
            });
        }
    }

    [HttpGet("GetPaymentData")]
    public async Task<IActionResult> GetPaymentData()
    {
        try
        {
            var paymentData = await _paymentService.GetPaymentData();
            return Ok(paymentData);

        }
        catch (Exception ex)
        {
            return BadRequest(new PaymentResponseModel
            {
                IsSuccess = false,
                Message = ex.ToString(),
                Data = null
            });
        }
    }


}
