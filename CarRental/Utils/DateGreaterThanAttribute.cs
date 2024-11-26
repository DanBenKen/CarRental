using System.ComponentModel.DataAnnotations;

namespace CarRental.Utils
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
        {
            var currentValue = (DateTime?)value;
            var comparisonProperty = validationContext.ObjectType.GetProperty(_comparisonProperty);

            if (comparisonProperty == null)
            {
                throw new ArgumentException("Comparison property not found.");
            }

            var comparisonValue = (DateTime?)comparisonProperty.GetValue(validationContext.ObjectInstance);

            if (!comparisonValue.HasValue || !currentValue.HasValue) 
            {
                return new ValidationResult("Both dates must be valid.");
            }

            if (currentValue.Value <= comparisonValue.Value)
            {
                return new ValidationResult("End date must be after the start date.");
            }

            return ValidationResult.Success;
        }
    }
}
