using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IFlightRepository
    {
        public List<Flight> GetFlights();
        public void AddPassengerToSeat(Flight flight);
        public void RemovePassengerFromSeat(Flight flight);
        public bool AddFlights(List<Flight> flights);
    }
}
