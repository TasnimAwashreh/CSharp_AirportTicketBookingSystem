using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using AirportTicketBookingExercise.Data.Repository;
using ATB.Data.Models;
using ATB.Logic.Enums;
using Microsoft.VisualBasic.FileIO;

namespace ATB.Logic.Service
{
    public class FlightService : IFlightService
    {
        private IFlightRepository _flightRepo;

        public FlightService(IFlightRepository fightRepo)
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
            return _flightRepo.getFlights();
        }

        public Flight? GetFlightById(int flightid)
        {
            return _flightRepo.getFlightById(flightid);
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
