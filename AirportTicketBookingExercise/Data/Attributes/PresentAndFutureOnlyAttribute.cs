using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATB.Data.Attributes
{
    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    public class PresentAndFutureOnlyAttribute : Attribute
    {
        public bool IsValid(object value)
        {
            if (value is DateTime dt)
            {
                return dt >= DateTime.Now;
            }

            return false; 
        }

        public string FormatErrorMessage(string name)
        {
            return $"{name} must be in the present or future.";
        }


    }
}
