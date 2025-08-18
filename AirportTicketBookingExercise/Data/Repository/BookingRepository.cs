using ATB.Data.Db.DAOs;
using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public class BookingRepository : IBookingRepository
    {
        private readonly BookingDAO _dao;

        public BookingRepository(BookingDAO dao) 
        {
            this._dao = dao;
        }

        public bool CreateBooking(Booking booking)
        {
            try
            {
                _dao.CreateBooking(booking);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Booking> GetBookingsByUserId(int PassengerId)
        {
            List<Booking> bookings = _dao.GetBookingsByUserId(PassengerId);
            return bookings;
        }

        public List<Booking> GetAllBookings()
        {
            List<Booking> bookings = _dao.GetAllBookings();
            return bookings;
        }

        public bool ValidatePassengerBooking(int bookingId, int passengerId)
        {
            return _dao.ValidateBooking(bookingId, passengerId);
        }

        public bool DeleteBooking(int bookingId)
        {
            return _dao.DeleteBookingById(bookingId);
        }

    }
}
