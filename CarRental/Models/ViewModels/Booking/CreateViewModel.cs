using System.ComponentModel.DataAnnotations;
using CarRental.Models.Validation;
using CarRental.Utils;

namespace CarRental.Models.ViewModels.Booking
{
    public class CreateViewModel
    {
        [Required(ErrorMessage = "UserId is required.")]
        public int UserId { get; set; }

        [Required(ErrorMessage = "CarId is required.")]
        [CarAvailable(ErrorMessage = "The vehicle is already reserved for the selected time period.")]
        public int CarId { get; set; }

        public string Make { get; set; }

        public string Model { get; set; }

        [Range(1, double.MaxValue, ErrorMessage = "Total price must be greater then 0.")]
        public decimal PricePerDay { get; set; }

        [Required(ErrorMessage = "Start date is required.")]
        [DataType(DataType.Date)]
        [Display(Name = "Start Date")]
        [FutureDate(ErrorMessage = "Start date must be in present or future.")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required.")]
        [Display(Name = "End Date")]
        [DateGreaterThan(nameof(StartDate), ErrorMessage = "End date must be after the start date.")]
        public DateTime EndDate { get; set; }
    }
}
