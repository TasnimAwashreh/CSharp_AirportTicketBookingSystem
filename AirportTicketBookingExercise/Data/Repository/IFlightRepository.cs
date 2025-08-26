using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IFlightRepository
    {
        public string ValidateCSVData();
        public bool ImportCSVData();
        public List<Flight> GetFlights();
        public void AddPassengerToSeat(Flight flight);
        public void RemovePassengerFromSeat(Flight flight);
    }
}
