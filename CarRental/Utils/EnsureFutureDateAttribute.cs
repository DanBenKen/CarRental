using System.ComponentModel.DataAnnotations;

namespace CarRental.Utils
{
    public class EnsureFutureDateAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            if (value is DateTime date)
            {
                if (date < DateTime.Today) return new ValidationResult("Start date must be in present or future.");

                return ValidationResult.Success;
            }

            return new ValidationResult("Invalid date format.");
        }
    }
}
