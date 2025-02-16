using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.TravelersListByBookingId
{
    public class TravelersListResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
    public class TravelerDataRequestModel
    {
        public User UserId { get; set; } = null!;
        public TravelPackage TravelPackageId { get; set; } = null!;
        public int NumberOfTravelers { get; set; }
    }
}
