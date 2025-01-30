using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TravelAgency.Domain.Features.AddTraveler;

public class AddTravelerRequestModel
{

	public string Name { get; set; } = null!;

	public int Age { get; set; }

	public string Gender { get; set; } = null!;
}
