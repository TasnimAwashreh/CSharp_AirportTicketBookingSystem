using ATB.Data.Models;

namespace AirportTicketBookingExercise.Data.Repository
{
    public interface IBookingsFilterRepository
    {
        public List<Booking> FilterBookingsWithFlights(string[] filterInput);
    }
}
