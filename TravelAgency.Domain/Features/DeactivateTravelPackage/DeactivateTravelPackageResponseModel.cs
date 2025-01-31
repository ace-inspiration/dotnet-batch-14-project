using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.DeactivateTravelPackage;
public class DeactivateTravelPackageResponseModel
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public object Data { get; set; }
}

