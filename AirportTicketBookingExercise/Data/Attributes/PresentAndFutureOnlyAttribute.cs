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
                DateTime datetime = DateTime.ParseExact(value.ToString(), "dd/MM/yy", CultureInfo.InvariantCulture);
                if (datetime == null)
                    return false;
                else if (datetime is DateTime date)
                    return date.Date >= DateTime.Today;
                else return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
