using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public interface IFlightservice
    {
        void AddPassengerToSeat(Flight flight);
        Flight? GetFlight(int flightid);
        List<Flight> GetFlights();
        bool ImportFlightData();
        void RemovePassengerToSeat(Flight flight);
        string ValidateFlightData();
    }
}