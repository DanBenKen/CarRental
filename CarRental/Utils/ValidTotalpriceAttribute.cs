using System.ComponentModel.DataAnnotations;
using CarRental.Utils;
using CarRental.Models.ViewModels.Booking;
using CarRental;

public class ValidTotalPriceAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var dbContext = (CarRentalContext)validationContext.GetService(typeof(CarRentalContext));
        if (dbContext == null) return new ValidationResult("DbContext service is not available.");

        var booking = (CreateViewModel)validationContext.ObjectInstance;

        var calculatedTotalPrice = CalculateTotalPrice(booking.StartDate, booking.EndDate, booking.PricePerDay);
        if (calculatedTotalPrice != booking.TotalPrice) return new ValidationResult("The total price does not match the calculated value.");

        return ValidationResult.Success;
    }

    public decimal CalculateTotalPrice(DateTime startDate, DateTime endDate, decimal pricePerDay)
    {
        var totalDays = (endDate - startDate).Days;

        if (totalDays < 1) totalDays = 1;

        var totalPrice = totalDays * pricePerDay;

        return totalPrice;
    }
}
