using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.TravelersListByBookingId
{
    public class TravelersListRequestModel
    {
        public string UserId { get; set; } = null!;
        public string TravelPackageId { get; set; } = null!;
        public int NumberOfTravelers { get; set; }
    }
    public class Travelerdata
    {
        public Booking booking { get; set; } = null!;
        public TravelPackage TravelPackage { get; set; } = null!;
        public string Name { get; set; } = null!;
        public int Age { get; set; }
        public string Gender { get; set; } = null!;
    }
}
