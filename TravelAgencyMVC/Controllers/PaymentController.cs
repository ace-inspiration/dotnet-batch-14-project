using Microsoft.AspNetCore.Mvc;
using TravelAgency.Domain.Features.PaymentFeature;

namespace TravelAgencyMVC.Controllers
{
    public class PaymentController : Controller
    {
        private readonly PaymentService _paymentService;
        public PaymentController(PaymentService paymentService)
        {
            _paymentService = paymentService;
        }

	
		// GET: Payment/Create
		[ActionName("Create")]
		public IActionResult CreatePayment()
		{
			return View("CreatePayment");
		}

		// POST: Payment/Create
		[HttpPost]
		[ActionName("Save")]
		public async Task<IActionResult> SaveBlog(PaymentRequestModel model)
		{
			if (ModelState.IsValid)
			{
				var response = await _paymentService.Execute(model);
				if (response.IsSuccess)
				{
					return RedirectToAction(nameof(Index));
				}
				ModelState.AddModelError("", response.Message);
			}
			return View(model);
		}

	}
}

