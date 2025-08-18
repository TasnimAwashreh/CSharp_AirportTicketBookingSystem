using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IFlightRepository
    {
        public string ValidateCSVData();
        public bool ImportCSVData();
        public List<Flight> getFlights();
        public Flight? getFlightById(int flightId);
        public void AddPassengerToSeat(Flight flight);
        public void RemovePassengerFromSeat(Flight flight);
        public Flight GetFlightByName(string name);
    }
}
