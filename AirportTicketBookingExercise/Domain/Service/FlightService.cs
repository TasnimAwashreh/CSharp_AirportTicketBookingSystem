using ATB.Data.Repository;
using ATB.Data.Models;
using System.ComponentModel.DataAnnotations;
using System.Text;
using ATB.Logic.Enums;
using AirportTicketBookingExercise.Logic.Utils;
using AirportTicketBookingExercise.Domain.Models;

namespace ATB.Logic.Service
{
    public class FlightService : IFlightService
    {
        private IFlightRepository _flightRepo;

        public FlightService(IFlightRepository fightRepo)
        {
            _flightRepo = fightRepo;
        }

        public List<Flight> GetFlights()
        {
            return _flightRepo.GetFlights();
        }

        public string ValidateFlightData(string importPath)
        {
            var strBuilder = new StringBuilder("");
            int rowCount = 1;
            var flights = CsvActionsHelper.GetAllRecords<Flight, FlightMap>(importPath);

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

        public List<Flight> ImportFlightData(string importPath)
        {
            List<Flight> importedFlightData = new List<Flight>();
            importedFlightData = CsvActionsHelper.GetAllRecords<Flight, FlightMap>(importPath);
            
            foreach (var flight in importedFlightData)
            {
                var context = new ValidationContext(flight, null, null);
                var validationResults = new List<ValidationResult>();
                bool isFieldValid = Validator.TryValidateObject(flight, context, validationResults, true);

                if (!isFieldValid)
                    throw new FormatException();
            }

            _flightRepo.AddFlights(importedFlightData);
            return importedFlightData;
        }

        public List<Flight> Search(string[] searchInput)
        {
            BookingFilter query = BookingFilters.Parse(searchInput.Skip(1).ToArray());
            return _flightRepo.FilterFlights(query);
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
