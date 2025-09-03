using ATB.Data.Models;
using ATB.Logic.Enums;

namespace ATB.Logic.Service
{
    public interface IFlightservice
    {
        List<Flight> GetFlights();
        public string ValidateFlightData(string importPath);
        public bool ImportFlightData(string importPath);
        public List<Flight> Search(FilterParam searchParam, string valueParam);
        public string FlightsToString(List<Flight> Flights);

    }
}