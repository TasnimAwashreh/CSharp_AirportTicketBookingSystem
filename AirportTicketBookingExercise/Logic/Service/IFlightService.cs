using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public interface IFlightservice
    {
        void AddPassengerToSeat(Flight flight);
        Flight? GetFlight(int flightid);
        List<Flight> GetFlights();
        void RemovePassengerToSeat(Flight flight);
        public string ValidateFlightData(string importPath);
        public bool ImportFlightData(string importPath);

    }
}