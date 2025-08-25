
using ATB.Data.Repository;
using ATB.Data.Models;

namespace ATB.Logic.Service
{
    public class Flightservice : IFlightservice
    {
        private IFlightRepository _flightRepo;

        public Flightservice(IFlightRepository fightRepo)
        {
            _flightRepo = fightRepo;
        }

        public string ValidateFlightData()
        {
            return _flightRepo.ValidateCSVData();
        }

        public bool ImportFlightData()
        {
            return _flightRepo.ImportCSVData();
        }

        public List<Flight> GetFlights()
        {
            return _flightRepo.GetFlights();
        }

        public Flight? GetFlight(int flightid)
        {
            return _flightRepo.GetFlight(flightid);
        }

        public void AddPassengerToSeat(Flight flight)
        {
            _flightRepo.AddPassengerToSeat(flight);
        }

        public void RemovePassengerToSeat(Flight flight)
        {
            _flightRepo.RemovePassengerFromSeat(flight);
        }

    }
}
