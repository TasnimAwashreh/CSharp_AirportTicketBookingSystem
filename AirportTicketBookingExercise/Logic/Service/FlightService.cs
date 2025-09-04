using ATB.Data.Repository;
using ATB.Data.Models;
using Microsoft.VisualBasic.FileIO;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text;
using ATB.Logic.Enums;
using CsvHelper;
using AirportTicketBookingExercise.Logic.Utils;

namespace ATB.Logic.Service
{
    public class Flightservice : IFlightservice
    {
        private IFlightRepository _flightRepo;

        public Flightservice(IFlightRepository fightRepo)
        {
            _flightRepo = fightRepo;
        }

        public List<Flight> GetFlights()
        {
            List<Flight> flights = new List<Flight>();
            flights = _flightRepo.GetFlights();
            return flights;
        }

        public string ValidateFlightData(string importPath)
        {
            var strBuilder = new StringBuilder("");
            int rowCount = 1;
            var flights = new List<Flight>();
            flights = CsvActionsHelper.GetAllRecords<Flight, FlightMap>(importPath);

                foreach (var flight in flights)
                {
                    var context = new ValidationContext(flight, null, null);
                    var validationResults = new List<ValidationResult>();
                    bool isFieldValid = Validator.TryValidateObject(flight, context, validationResults, true);

                    if (!isFieldValid)
                    {
                        foreach (var vr in validationResults)
                            strBuilder.AppendLine($"Row {rowCount}: {vr.ErrorMessage}");
                    }
                }

            return strBuilder.ToString();
        }

        public bool ImportFlightData(string importPath)
        {
            List<Flight> importedFlightData = new List<Flight>();
            CsvActionsHelper.GetAllRecords<Flight, FlightMap>(importPath);
            
            foreach (var flight in importedFlightData)
            {
                var context = new ValidationContext(flight, null, null);
                var validationResults = new List<ValidationResult>();
                bool isFieldValid = Validator.TryValidateObject(flight, context, validationResults, true);

                if (!isFieldValid)
                    return false;
            }

            _flightRepo.AddFlights(importedFlightData);
            return true;
        }

        private Flight? AddFlight(string[] fields)
        {
            var flight = new Flight
            {
                FlightId = int.Parse(fields[0]),
                FlightName = fields[1],
                DepartureCountry = fields[2],
                DestinationCountry = fields[3],
                DepartureAirport = fields[4],
                ArrivalAirport = fields[5],
                DepartureDate = DateTime.ParseExact(
                                    fields[6],
                                    new[] { "MM/dd/yyyy", "M/d/yyyy", "M/dd/yyyy", "MM/d/yyyy" },
                                    CultureInfo.InvariantCulture,
                                    DateTimeStyles.None),
                EconomyPrice = Decimal.Parse(fields[7]),
                BuisnessPrice = Decimal.Parse(fields[8]),
                FirstClassPrice = Decimal.Parse(fields[9]),
                SeatCapacity = int.Parse(fields[10])
            };
            return flight;
        }

        public List<Flight> Search(FilterParam searchParam, string valueParam)
        {
            return _flightRepo.Search(searchParam, valueParam);
        }

        public string FlightsToString(List<Flight> Flights)
        {
            StringBuilder stringBuilder = new StringBuilder();
            foreach (var flight in Flights)
            {
                stringBuilder.AppendLine(flight.ToString());
            }
            return stringBuilder.ToString();
        }
    }
}
