using System.ComponentModel.DataAnnotations;
using CarRental.Utils;
using CarRental.Models.ViewModels.Booking;

public class ValidTotalPriceAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var garage = validationContext.GetService(typeof(Garage)) as Garage;
        if (garage == null)
        {
            return new ValidationResult("Garage service is not available.");
        }

        var booking = (CreateViewModel)validationContext.ObjectInstance;

        var calculatedTotalPrice = garage.CalculateTotalPrice(booking.StartDate, booking.EndDate, booking.PricePerDay);
        if (calculatedTotalPrice != booking.TotalPrice)
        {
            return new ValidationResult("The total price does not match the calculated value.");
        }

        return ValidationResult.Success;
    }
}
