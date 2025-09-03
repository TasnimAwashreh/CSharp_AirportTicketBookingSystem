using ATB.Data.Models;
using ATB.Logic;
using ATB.Logic.Enums;

namespace ATB.Data.Repository
{
    public interface IBookingRepository
    {
        public void CreateBooking(Booking booking);
        public List<Booking> GetAllBookings();
        public List<Booking> GetBookings(int passengerId);
        public bool IsBookingValidById(int bookingId, int passengerId);
        public bool DeleteBooking(int BookingId);
        public bool UpdateBookingClass(int bookingId, BookingClass newClass);
        public List<Booking> FilterBooking(BookingFilter filter);

    }
}
