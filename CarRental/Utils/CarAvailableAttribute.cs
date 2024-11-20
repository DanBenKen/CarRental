using System.ComponentModel.DataAnnotations;
using CarRental.Models.ViewModels.Booking;
using CarRental.Utils;

namespace CarRental.Models.Validation
{
    public class CarAvailableAttribute : ValidationAttribute
    {
        private readonly Garage _garage;

        public CarAvailableAttribute()
        {
            _garage = new Garage();
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var bookingModel = (CreateViewModel)validationContext.ObjectInstance;

            var existingBooking = _garage.GenerateBookings()
                .FirstOrDefault(b => b.CarId == bookingModel.CarId &&
                                     ((b.StartDate <= bookingModel.EndDate && b.StartDate >= bookingModel.StartDate) ||
                                      (b.EndDate >= bookingModel.StartDate && b.EndDate <= bookingModel.EndDate)));

            if (existingBooking != null)
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}
