namespace CarRental.Models.ViewModels.Car
{
    public class CarListViewModel
    {
        public IEnumerable<Models.Car> Cars { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public int TotalItems { get; set; }
    }
}
