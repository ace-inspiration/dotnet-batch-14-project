using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.BookingFeatures;

public class BookingRequestModel
{
    public string UserId { get; set; } = null!;
    public DateTime? TravelStartdate { get; set; }
    public string TravelPackageId { get; set; } = null!;
    public List<TravelerRequestModel> Travelers { get; set; } = new List<TravelerRequestModel>();
}

public class TravelerRequestModel
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Gender { get; set; } = null!;
}
