namespace CarRental.Models.ViewModels.Booking
{
    public class ReturnViewModel
    {
        public int BookingId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal PricePerDay { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
