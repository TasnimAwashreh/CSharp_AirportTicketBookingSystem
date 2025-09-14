using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace ATB.Data.Attributes
{
    [System.AttributeUsage(System.AttributeTargets.Field | System.AttributeTargets.Parameter | System.AttributeTargets.Property, AllowMultiple = false)]
    public class PresentAndFutureOnlyAttribute : ValidationAttribute
    {
        public PresentAndFutureOnlyAttribute()
        {
            ErrorMessage = "Invalid date.";
        }

        public override bool IsValid(object? value)
        {
            try
            {
                if (DateTime.TryParse(value.ToString(), out var date))
                {
                    return date.Date >= DateTime.Today;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
