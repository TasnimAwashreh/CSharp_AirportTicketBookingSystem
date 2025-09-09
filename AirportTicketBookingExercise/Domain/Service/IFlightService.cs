using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Logic.Service
{
    public interface IFlightservice
    {
        List<Flight> GetFlights();
        public string ValidateFlightData(string importPath);
        public bool ImportFlightData(string importPath);
        public string FlightsToString(List<Flight> Flights);
        public List<Flight> Search(string[] searchInput);

    }
}