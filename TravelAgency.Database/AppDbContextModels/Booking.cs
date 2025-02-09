using System;
using System.Collections.Generic;

namespace TravelAgency.Database.AppDbContextModels;

public partial class Booking
{
    public string Id { get; set; } = null!;

    public string UserId { get; set; } = null!;

    public string TravelPackageId { get; set; } = null!;

    public int NumberOfTravelers { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime BookingDate { get; set; }

    public DateTime? TravelStartdate { get; set; }

    public DateTime? TravelEnddate { get; set; }

    public string Status { get; set; } = null!;
}
