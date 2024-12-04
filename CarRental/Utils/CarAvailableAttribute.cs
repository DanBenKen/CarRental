using System.ComponentModel.DataAnnotations;
using CarRental.Models.ViewModels.Booking;

namespace CarRental.Models.Validation
{
    public class CarAvailableAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var dbContext = (CarRentalContext)validationContext.GetService(typeof(CarRentalContext));
            if (dbContext == null) return new ValidationResult("DbContext service is not available.");

            var bookingModel = (CreateViewModel)validationContext.ObjectInstance;

            var existingBooking = dbContext.Bookings
                .Where(b => b.CarId == bookingModel.CarId &&
                            ((b.StartDate <= bookingModel.EndDate && b.StartDate >= bookingModel.StartDate) ||
                             (b.EndDate >= bookingModel.StartDate && b.EndDate <= bookingModel.EndDate)) &&
                             b.Status == "Rented")
                .FirstOrDefault();

            if (existingBooking != null) return new ValidationResult("The vehicle is already reserved for the selected time period.");

            return ValidationResult.Success;
        }
    }
}
