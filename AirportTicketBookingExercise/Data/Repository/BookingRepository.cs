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

        public bool CreateBooking(Booking Booking)
        {
            try
            {
                _dao.CreateBooking(Booking);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public List<Booking> GetBookingsByUserId(int PassengerId)
        {
            List<Booking> Bookings = _dao.GetBookingsByUserId(PassengerId);
            return Bookings;
        }

        public List<Booking> GetAllBookings()
        {
            List<Booking> Bookings = _dao.GetAllBookings();
            return Bookings;
        }

        public bool ValidatePassengerBooking(int BookingId, int passengerId)
        {
            return _dao.ValidateBooking(BookingId, passengerId);
        }

        public bool DeleteBooking(int BookingId)
        {
            return _dao.DeleteBookingById(BookingId);
        }

    }
}
