using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IBookingRepository
    {
        public bool CreateBooking(Booking Booking);
        public List<Booking> GetBookingsByUserId(int PassengerId);
        public List<Booking> GetAllBookings();
        public bool ValidatePassengerBooking(int BookingId, int passengerId);
        public bool DeleteBooking(int BookingId);

    }
}
