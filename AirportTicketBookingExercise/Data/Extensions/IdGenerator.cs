using ATB.Data.Models;

namespace ATB.Data.Extensions
{
    public static class IdGenerator
    {
        private static readonly Random _random = new Random();
        public static void GenerateUserId(this User user)
        {
            user.UserId = _random.Next(100000, 999999);
        }

        public static void GenerateBookingId(this Booking booking)
        {
            booking.BookingId = _random.Next(100000, 999999);
        }
    }
}
