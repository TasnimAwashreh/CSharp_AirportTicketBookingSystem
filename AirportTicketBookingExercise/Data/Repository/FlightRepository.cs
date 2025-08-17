using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Reflection;
using System.Text;
using AirportTicketBookingExercise.Data.Repository;
using ATB.Data.Models;
using Microsoft.VisualBasic.FileIO;


namespace ATB.Data.Repository
{
    public class FlightRepository: IFlightRepository
    {
        string path = $"C:/Users/TasnimAwashreh/Desktop/Learning/FTSBackendEngineerNotes/2_CSHARP/AirportTicketBookingExercise/Flights.csv";
        private List<Flight> _flights;

        public FlightRepository()
        {
            _flights = new List<Flight>();
        }

        public List<Flight> getFlights() { return _flights; }

        public Flight? getFlightById(int flightId)
        {
            return _flights.Find(flight => flight.FlightId == flightId);
        }

        public Flight GetFlightByName(string? name)
        {
            return _flights.Find(flight => flight.FlightName.ToLowerInvariant() == name.ToLowerInvariant());
        }

        public void AddPassengerToSeat(Flight flight)
        {
            flight.seatsAvailable++;
        }

        public void RemovePassengerFromSeat(Flight flight)
        {
            flight.seatsAvailable--;
        }

        private void validateField<T>(string fieldName, string fieldInput, 
            Func<string, T> parseValue, StringBuilder strBuilder, int rowCount)
        {
            PropertyInfo property = typeof(Flight).GetProperty(fieldName);
            var attributes = property.GetCustomAttributes(typeof(ValidationAttribute), true);
            try
            {
                T value;
                if (typeof(T) == typeof(DateTime))
                    DateTime.ParseExact(fieldInput, "dd/MM/yy", CultureInfo.InvariantCulture);
                else
                    value = parseValue(fieldInput);
                foreach (ValidationAttribute attr in attributes)
                {
                    if (!attr.IsValid(fieldInput))
                        strBuilder.Append($"Row {rowCount}: {attr.ErrorMessage} \n");
                }
            }
            catch (Exception e) {
                strBuilder.Append($"Error at row {rowCount}: Wrong Format for field {fieldName} \n");
            }
        }

        public string ValidateCSVData()
        {
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.ReadLine();
                StringBuilder strBuilder = new StringBuilder("");
                int rowCount = 1;
                while (!csvParser.EndOfData)
                {
                    string[] fields = csvParser.ReadFields();
                    validateField("FlightId", fields[0], int.Parse, strBuilder, rowCount);
                    validateField("FlightName", fields[1], x => x, strBuilder, rowCount);
                    validateField("DepartureCountry", fields[2], x => x, strBuilder, rowCount);
                    validateField("DestinationCountry", fields[3], x => x, strBuilder, rowCount);
                    validateField("DepartureDate", fields[4], DateTime.Parse, strBuilder, rowCount);
                    validateField("DepartureAirport", fields[5], x => x, strBuilder, rowCount);
                    validateField("ArrivalAirport", fields[6], x => x, strBuilder, rowCount);
                    validateField("EconomyPrice", fields[7], Decimal.Parse, strBuilder, rowCount);
                    validateField("BuisnessPrice", fields[8], Decimal.Parse, strBuilder, rowCount);
                    validateField("FirstClassPrice", fields[9], Decimal.Parse, strBuilder, rowCount);
                    validateField("SeatCapacity", fields[10], int.Parse, strBuilder, rowCount);
                    rowCount++;
                }
                return strBuilder.ToString();
            }
        }

        public bool ImportCSVData()
        {
            List<Flight> flightData = new List<Flight>();
            using (TextFieldParser csvParser = new TextFieldParser(path))
            {
                csvParser.CommentTokens = new string[] { "#" };
                csvParser.SetDelimiters(new string[] { "," });
                csvParser.HasFieldsEnclosedInQuotes = true;
                csvParser.ReadLine();
                try
                {
                    while (!csvParser.EndOfData)
                    {
                        string[] fields = csvParser.ReadFields();
                        Flight flight = new Flight
                        {
                            FlightId = int.Parse(fields[0]),
                            FlightName = fields[1],
                            DepartureCountry = fields[2],
                            DestinationCountry = fields[3],
                            DepartureDate = DateTime.ParseExact(fields[4], "dd/MM/yy", CultureInfo.InvariantCulture),
                            DepartureAirport = fields[5],
                            ArrivalAirport = fields[6],
                            EconomyPrice = Decimal.Parse(fields[7]),
                            BuisnessPrice = Decimal.Parse(fields[8]),
                            FirstClassPrice = Decimal.Parse(fields[9]),
                            SeatCapacity = int.Parse(fields[10])
                        };
                        flightData.Add(flight);
                    }
                    _flights = flightData;
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }






    }
}
