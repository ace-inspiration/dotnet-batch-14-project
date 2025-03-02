using System;
using System.Collections.Generic;

namespace TravelAgency.Database.AppDbContextModels;

public partial class User
{
    public string Id { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Role { get; set; } = null!;
    public string? Status { get; set; } 
    public string? OTP { get; set; } 
    public DateTime OTP_Expiry { get; set; }
}
