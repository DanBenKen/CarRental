using System.ComponentModel.DataAnnotations;
using CarRental.Models.ViewModels.Booking;
using Newtonsoft.Json;

namespace CarRental.Models.Validation
{
    public class CarAvailableAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            IHttpContextAccessor? httpContext = validationContext.GetService(typeof(IHttpContextAccessor)) as IHttpContextAccessor;
            if (httpContext == null) return new ValidationResult("Unable to validate car availability due to missing HTTP context.");

            var session = httpContext.HttpContext?.Session;
            if (session == null) return new ValidationResult("Session is not available.");

            var bookingsJson = session.GetString("Bookings");
            if (string.IsNullOrEmpty(bookingsJson)) return ValidationResult.Success;

            var bookings = JsonConvert.DeserializeObject<List<Booking>>(bookingsJson);

            var bookingModel = (CreateViewModel)validationContext.ObjectInstance;
            var existingBooking = bookings.FirstOrDefault(b => b.CarId == bookingModel.CarId &&
                                                               ((b.StartDate <= bookingModel.EndDate && b.StartDate >= bookingModel.StartDate) ||
                                                                (b.EndDate >= bookingModel.StartDate && b.EndDate <= bookingModel.EndDate)));

            if (existingBooking != null)
            {
                return new ValidationResult("The vehicle is already reserved for the selected time period.");
            }

            return ValidationResult.Success;
        }
    }
}
