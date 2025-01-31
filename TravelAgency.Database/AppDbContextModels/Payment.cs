using System;
using System.Collections.Generic;

namespace TravelAgency.Database.AppDbContextModels;


public partial class Payment
{
    public string Id { get; set; } = null!;

    public string BookingId { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string PaymentStatus {   get; set; } = null!;
}
