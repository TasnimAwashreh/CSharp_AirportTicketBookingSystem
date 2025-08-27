namespace ATB.Logic.Enums
{
    public enum BookingClass
    {
        None = 0,
        First = 1,
        Business = 2,
        Economy = 3
    }

    public static class BookingClasses
    {
        public static BookingClass ParseBookingClass(this string bookingClassStr)
        {
            switch (bookingClassStr.ToLower())
            {
                case "first": return BookingClass.First;
                case "business": return BookingClass.Business;
                case "economy": return BookingClass.Economy;
                default: return BookingClass.None;
            }
        }
    }
}
