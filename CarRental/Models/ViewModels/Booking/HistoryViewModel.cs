namespace CarRental.Models.ViewModels.Booking
{
    public class HistoryViewModel
    {
        public int BookingId { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public decimal TotalPrice { get; set; }
        public string Status { get; set; }
    }
}
