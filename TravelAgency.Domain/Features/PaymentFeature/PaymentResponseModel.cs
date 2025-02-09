using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TravelAgency.Database.AppDbContextModels;

namespace TravelAgency.Domain.Features.PaymentFeature;

public class PaymentResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public Payment Data { get; set; }
} 
public class PaymentListResponseModel
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public List<Payment> Data { get; set; }
}

public class PaymentData 
{
    public string Id { get; set; } = null!;

    public User User { get; set; } = null!;

    public Booking Booking { get; set; } = null!;

    public TravelPackage TravelPackage { get; set; } = null!;

    public decimal Amount { get; set; }

    public DateTime PaymentDate { get; set; }

    public string? PaymentType { get; set; }

    public string PaymentStatus { get; set; } = null!;

}


