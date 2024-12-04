namespace CarRental.Models.ViewModels.Car
{
    public class IndexViewModel
    {
        public IEnumerable<Models.Car> Cars { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
    }
}
