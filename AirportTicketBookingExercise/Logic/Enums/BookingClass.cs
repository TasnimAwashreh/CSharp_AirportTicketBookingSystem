

namespace ATB.Logic.Enums
{
    public enum BookingClass
    {
        first,
        business,
        economy,
        none
    }
    public class BookingClasses
    {
        public static BookingClass strToBookingClass(string bookingClass)
        {
            switch (bookingClass.ToLower())
            {
                case "first": return BookingClass.first;
                case "business": return BookingClass.business;
                case "economy": return BookingClass.economy;
                default: return BookingClass.none;
            }
        }
    }
}
