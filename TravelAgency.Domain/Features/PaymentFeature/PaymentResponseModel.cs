using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.PaymentFeature
{
    public class PaymentResponseModel
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }
        public Payment Data { get; set; }
    }
}
