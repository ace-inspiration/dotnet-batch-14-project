using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.BookingFeatures;

public class BookingRequestModel
{
    public string UserId { get; set; } = null!;
    public string TravelPackageId { get; set; } = null!;
    public int NumberOfTravelers { get; set; }

}
