namespace CarRental.Models.ViewModels.Booking
{
    public class PaginatedHistoryViewModel
    {
        public IEnumerable<HistoryViewModel> Bookings { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
