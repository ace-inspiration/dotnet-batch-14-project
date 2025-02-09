using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;


namespace TravelAgency.Domain.Features.BookingFeatures;

public class BookingResponseModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}


public class bookdata
{
    public string Id { get; set; } = null!;

    public User User { get; set; } = null!;

    public TravelPackage TravelPackage { get; set; } = null!;

    public int NumberOfTravelers { get; set; }

    public decimal TotalPrice { get; set; }

    public DateTime BookingDate { get; set; }

    public DateTime? TravelStartdate { get; set; }

    public DateTime? TravelEnddate { get; set; }

    public string Status { get; set; } = null!;
}

