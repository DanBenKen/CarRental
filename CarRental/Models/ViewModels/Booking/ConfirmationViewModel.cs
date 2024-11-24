namespace CarRental.Models.ViewModels.Booking
{
    public class ConfirmationViewModel
    {
        public int BookingId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
    }
}
