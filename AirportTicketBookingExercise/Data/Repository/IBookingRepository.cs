using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Data.Repository
{
    public interface IBookingRepository
    {
        public bool CreateBooking(Booking booking);
        public List<Booking> GetAllBookings();
        public bool DeleteBooking(int BookingId);
        public bool UpdateBookingClass(int bookingId, BookingClass newClass);

    }
}
