using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IFlightRepository
    {
        public List<Flight> GetFlights();
        public Flight? GetFlight(int flightId);
        public void AddPassengerToSeat(Flight flight);
        public void RemovePassengerFromSeat(Flight flight);
        public void AddFlights(List<Flight> flights);
    }
}
