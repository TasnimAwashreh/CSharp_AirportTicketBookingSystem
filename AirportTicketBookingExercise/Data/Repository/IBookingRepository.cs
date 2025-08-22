using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IBookingRepository
    {
        public bool CreateBooking(Booking Booking);
        public List<Booking> GetBookings(int PassengerId);
        public List<Booking> GetAllBookings();
        public bool IsPassengerBookingValid(int BookingId, int passengerId);
        public bool DeleteBooking(int BookingId);

    }
}
