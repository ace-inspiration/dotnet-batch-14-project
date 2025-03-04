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
    public DateTime? TravelEnddate { get; set; }
    public string TravelPackageId { get; set; } = null!;
    public List<TravelerRequestModel> Travelers { get; set; } = new List<TravelerRequestModel>();
}

public class TravelerRequestModel
{
    public string Name { get; set; } = null!;
    public int Age { get; set; }
    public string Gender { get; set; } = null!;
}
public class HomeModel
{
    public int countallbooking { get; set; } = 0 ;
    public int countallpayment { get; set; } = 0;
    public int countalltraveler { get; set; } = 0;
    public int countalluser { get; set; } = 0;
    public int countalltravelPackage { get; set; } = 0;
    public int countverifybooking { get; set; } = 0;
    public int countverifiedbooking { get; set; } = 0;
    public int countverifypayment { get; set; } = 0;
    public int countverifiedpayment { get; set; } = 0;
    public int countcompletepayment { get; set; } = 0;
    public int counttodaybookingPackage { get; set; } = 0;
    public int counttodayBooking { get; set; } = 0;
    public int counttodayPayment { get; set; } = 0;
    public int[] bookingweekdata { get; set; }= new int[7];
    public int[] paymentweekdata { get; set; }= new int[7];
 
}