using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Logic.Service
{
    public interface IFlightService
    {
        List<Flight> GetFlights();
        public string ValidateFlightData(string importPath);
        public List<Flight> ImportFlightData(string importPath);
        public string FlightsToString(List<Flight> Flights);
        public List<Flight> Search(string[] searchInput);

    }
}