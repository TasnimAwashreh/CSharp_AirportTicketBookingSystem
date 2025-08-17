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
        /*
                public List<Flight> Filter<T>(FilterParam command, string input)
                {
                    List<Flight> flights = _flightRepo.getFlights();
                    try
                    {

                        switch (command)
                        {
                            case FilterParam.flight:
                                return flights.Where(flight => flight.FlightName.Equals(input)).ToList();
                            case FilterParam.price:
                                decimal price = decimal.Parse(input);
                                return flights.Where(flight => flight.BuisnessPrice.Equals(price) || flight.EconomyPrice.Equals(price) 
                                || flight.FirstClassPrice.Equals(price)).ToList();
                            case FilterParam.departure_country:
                                return flights.Where(flight => flight.DepartureCountry.Equals(input)).ToList();
                            case FilterParam.destination_country:
                                return flights.Where(flight => flight.DestinationCountry.Equals(input)).ToList();
                            case FilterParam.departure_date:
                                return flights.Where(flight => flight.DepartureDate.Equals(value)).ToList();
                            case FilterParam.departure_airport:
                                return flights.Where(flight => flight.DepartureAirport.Equals(input)).ToList();
                            case FilterParam.arrival_airport:
                                return flights.Where(flight => flight.ArrivalAirport.Equals(input)).ToList();
                            case FilterParam.flight_class:
                                return [];
                            case FilterParam.passenger:
                                return flights.Where(flight => flight.DepartureCountry.Equals(input)).ToList();
                            default:
                                return flights;
                        }
                    }
                    catch (Exception ex) { return []; }

                }
        */



    }
}
