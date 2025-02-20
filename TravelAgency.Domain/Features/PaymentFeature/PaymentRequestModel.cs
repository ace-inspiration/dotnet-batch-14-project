using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.PaymentFeature
{
    public class PaymentRequestModel
    {
        public string BookingId { get; set; } = null!;
       
        public string paymentType { get; set; } = null!;

    }
}
