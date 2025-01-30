using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.AddTraveler;

public class AddTravelerResponseModel
{
	public bool Success { get; set; }
	public string Message { get; set; }
	public object Data { get; set; }
}
