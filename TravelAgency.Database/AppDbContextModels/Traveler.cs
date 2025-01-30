using System;
using System.Collections.Generic;

namespace TravelAgency.Database.AppDbContextModels;

public partial class Traveler
{
    public string Id { get; set; } = null!;

    public string BookingId { get; set; } = null!;

    public string Name { get; set; } = null!;

    public int Age { get; set; }

    public string Gender { get; set; } = null!;
}
