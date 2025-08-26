using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Data.Repository
{
    public interface IBookingRepository
    {
        public bool CreateBooking(Booking booking);
        public List<Booking> GetBookings(int PassengerId);
        public List<Booking> GetAllBookings();
        public bool IsPassengerBookingValid(int BookingId, int passengerId);
        public bool DeleteBooking(int BookingId);
        public bool UpdateBookingClass(int bookingId, BookingClass newClass);

    }
}
