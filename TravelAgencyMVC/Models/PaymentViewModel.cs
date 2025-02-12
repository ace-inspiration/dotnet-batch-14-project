namespace TravelAgencyMVC.Models
{
	public class PaymentViewModel
	{
		public string UserName { get; set; }
		public DateTime BookingDate { get; set; }
		public string TravelPackageTitle { get; set; }
		public decimal Amount { get; set; }
		public DateTime PaymentDate { get; set; }
		public string PaymentType { get; set; }
	}
}
