

namespace ATB.Data
{
    public class BookingFilter
    {
        public string? FlightName { get; set; }
        public decimal? Price { get; set; }
        public string? DepartureCountry { get; set; }
        public string? DestinationCountry { get; set; }
        public DateTime? DepartureDate { get; set; }
        public string? DepartureAirport { get; set; }
        public string? ArrivalAirport { get; set; }
        public string? PassengerName { get; set; }
        public string? BookingClass { get; set; }
    }
}
