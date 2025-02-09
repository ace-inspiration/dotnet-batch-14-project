using System;
using System.Collections.Generic;

namespace TravelAgency.Database.AppDbContextModels;

public partial class TravelPackage
{
    public string Id { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string Destination { get; set; } = null!;

    public string? Description { get; set; }

    public decimal Price { get; set; }

    public int Duration { get; set; }

    public string? Inclusions { get; set; }

    public string? CancellationPolicy { get; set; }

    public string? Image { get; set; }

    public string Status { get; set; } = null!;
}
