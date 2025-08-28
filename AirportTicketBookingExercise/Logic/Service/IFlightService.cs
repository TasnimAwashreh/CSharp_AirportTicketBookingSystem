using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public interface IFlightservice
    {
        List<Flight> GetFlights();
        public string ValidateFlightData(string importPath);
        public bool ImportFlightData(string importPath);
        public List<Flight> Search(string[] filterInfo);
        public string FlightsToString(List<Flight> Flights);

    }
}