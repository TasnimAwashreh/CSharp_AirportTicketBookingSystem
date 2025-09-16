using ATB.Data.Models;
using ATB.Logic.Enums;

namespace AirportTicketExercise.Test
{
    public class DummyData
    {
        public static List<Flight> ValidFlights = new List<Flight>
            {
                new Flight
                {
                    FlightId = 1,
                    FlightName = "SkyJet 101",
                    DepartureCountry = "Germany",
                    DestinationCountry = "France",
                    DepartureAirport = "Berlin Brandenburg",
                    ArrivalAirport = "Charles de Gaulle",
                    DepartureDate = DateTime.UtcNow.AddDays(3),
                    EconomyPrice = 120.50m,
                    BuisnessPrice = 350.00m,
                    FirstClassPrice = 750.00m,
                    SeatCapacity = 180,
                    SeatsAvailable = 150
                },
                new Flight
                {
                    FlightId = 2,
                    FlightName = "EuroFly 202",
                    DepartureCountry = "Spain",
                    DestinationCountry = "Italy",
                    DepartureAirport = "Madrid Barajas",
                    ArrivalAirport = "Rome Fiumicino",
                    DepartureDate = DateTime.UtcNow.AddDays(7),
                    EconomyPrice = 90.00m,
                    BuisnessPrice = 280.00m,
                    FirstClassPrice = 600.00m,
                    SeatCapacity = 200,
                    SeatsAvailable = 180
                },
                new Flight
                {
                    FlightId = 3,
                    FlightName = "AirConnect 303",
                    DepartureCountry = "Netherlands",
                    DestinationCountry = "United Kingdom",
                    DepartureAirport = "Amsterdam Schiphol",
                    ArrivalAirport = "London Heathrow",
                    DepartureDate = DateTime.UtcNow.AddDays(1),
                    EconomyPrice = 70.00m,
                    BuisnessPrice = 250.00m,
                    FirstClassPrice = 500.00m,
                    SeatCapacity = 150,
                    SeatsAvailable = 120
                },
                new Flight
                {
                    FlightId = 4,
                    FlightName = "TransEuro 404",
                    DepartureCountry = "Sweden",
                    DestinationCountry = "Norway",
                    DepartureAirport = "Stockholm Arlanda",
                    ArrivalAirport = "Oslo Gardermoen",
                    DepartureDate = DateTime.UtcNow.AddDays(10),
                    EconomyPrice = 85.00m,
                    BuisnessPrice = 260.00m,
                    FirstClassPrice = 520.00m,
                    SeatCapacity = 160,
                    SeatsAvailable = 140
                },
                new Flight
                {
                    FlightId = 5,
                    FlightName = "Baltic Wings 505",
                    DepartureCountry = "Poland",
                    DestinationCountry = "Lithuania",
                    DepartureAirport = "Warsaw Chopin",
                    ArrivalAirport = "Vilnius Airport",
                    DepartureDate = DateTime.UtcNow.AddDays(5),
                    EconomyPrice = 65.00m,
                    BuisnessPrice = 220.00m,
                    FirstClassPrice = 480.00m,
                    SeatCapacity = 140,
                    SeatsAvailable = 100
                }
            };

        public static List<Flight> InValidFlights = new List<Flight>
            {
                new Flight
                {
                    FlightId = 1,
                    FlightName = "",
                    DepartureCountry = "Germany",
                    DestinationCountry = "France",
                    DepartureAirport = "Berlin Brandenburg",
                    ArrivalAirport = "Charles de Gaulle",
                    DepartureDate = DateTime.UtcNow.AddDays(3),
                    EconomyPrice = 120.50m,
                    BuisnessPrice = 350.00m,
                    FirstClassPrice = 750.00m,
                    SeatCapacity = 180,
                    SeatsAvailable = 150
                },
                new Flight
                {
                    FlightId = 1,
                    FlightName = "EuroFly 202",
                    DepartureCountry = "",
                    DestinationCountry = "Italy",
                    DepartureAirport = "Madrid Barajas",
                    ArrivalAirport = "Rome Fiumicino",
                    DepartureDate = DateTime.UtcNow.AddDays(7),
                    EconomyPrice = 90.00m,
                    BuisnessPrice = 280.00m,
                    FirstClassPrice = 600.00m,
                    SeatCapacity = 200,
                    SeatsAvailable = 180
                },
                new Flight
                {
                    FlightId = 3,
                    FlightName = "AirConnect 303",
                    DepartureCountry = "Netherlands",
                    DestinationCountry = "United Kingdom",
                    DepartureAirport = "Amsterdam Schiphol",
                    ArrivalAirport = "London Heathrow",
                    DepartureDate = DateTime.UtcNow.AddDays(1),
                    EconomyPrice = 70.00m,
                    BuisnessPrice = 250.00m,
                    FirstClassPrice = 500.00m,
                    SeatCapacity = 150,
                    SeatsAvailable = 120
                },
                new Flight
                {
                    FlightId = 4,
                    FlightName = "TransEuro 404",
                    DepartureCountry = "Sweden",
                    DestinationCountry = "Norway",
                    DepartureAirport = "Stockholm Arlanda",
                    ArrivalAirport = "Oslo Gardermoen",
                    DepartureDate = DateTime.UtcNow.AddDays(10),
                    EconomyPrice = 85.00m,
                    BuisnessPrice = 260.00m,
                    FirstClassPrice = 520.00m,
                    SeatCapacity = 160,
                    SeatsAvailable = 140
                },
                new Flight
                {
                    FlightId = 5,
                    FlightName = "Baltic Wings 505",
                    DepartureCountry = "Poland",
                    DestinationCountry = "Lithuania",
                    DepartureAirport = "Warsaw Chopin",
                    ArrivalAirport = "Vilnius Airport",
                    DepartureDate = DateTime.UtcNow.AddDays(5),
                    EconomyPrice = 65.00m,
                    BuisnessPrice = 220.00m,
                    FirstClassPrice = 480.00m,
                    SeatCapacity = 140,
                    SeatsAvailable = 100
                }
            };

        public static Flight ValidFlight1 = new Flight
        {
            FlightId = 35,
            FlightName = "EuroFly 202",
            DepartureCountry = "Spain",
            DestinationCountry = "Italy",
            DepartureAirport = "Madrid Barajas",
            ArrivalAirport = "Rome Fiumicino",
            DepartureDate = DateTime.UtcNow.AddDays(7),
            EconomyPrice = 90.00m,
            BuisnessPrice = 280.00m,
            FirstClassPrice = 600.00m,
            SeatCapacity = 200,
            SeatsAvailable = 180
        };

        public static Flight ValidFlight2 = new Flight
        {
            FlightId = 89,
            FlightName = "AirConnect 303",
            DepartureCountry = "Netherlands",
            DestinationCountry = "United Kingdom",
            DepartureAirport = "Amsterdam Schiphol",
            ArrivalAirport = "London Heathrow",
            DepartureDate = DateTime.UtcNow.AddDays(1),
            EconomyPrice = 70.00m,
            BuisnessPrice = 250.00m,
            FirstClassPrice = 500.00m,
            SeatCapacity = 150,
            SeatsAvailable = 120
        };

        public static Flight ValidFlightEmptyEconomyPrice = new Flight
        {
            FlightId = 2,
            FlightName = "EuroFly 202",
            DepartureCountry = "Spain",
            DestinationCountry = "Italy",
            DepartureAirport = "Madrid Barajas",
            ArrivalAirport = "Rome Fiumicino",
            DepartureDate = DateTime.UtcNow.AddDays(7),
            EconomyPrice = 0.00m,
            BuisnessPrice = 280.00m,
            FirstClassPrice = 600.00m,
            SeatCapacity = 200,
            SeatsAvailable = 180
        };

        public static Flight ValidFlightFullSeats = new Flight
        {
            FlightId = 2,
            FlightName = "EuroFly 202",
            DepartureCountry = "Spain",
            DestinationCountry = "Italy",
            DepartureAirport = "Madrid Barajas",
            ArrivalAirport = "Rome Fiumicino",
            DepartureDate = DateTime.UtcNow.AddDays(7),
            EconomyPrice = 90.00m,
            BuisnessPrice = 280.00m,
            FirstClassPrice = 600.00m,
            SeatCapacity = 200,
            SeatsAvailable = 200
        };


        public static User ValidUser1 = new User
        {
            UserId = 292,
            Name = "MrPassenger",
            Password = "Password123",
            UserType = UserType.Passenger
        };

        public static User ValidUser2 = new User
        {
            UserId = 298,
            Name = "MsPassenger",
            Password = "!MyPassword246",
            UserType = UserType.Passenger
        };

        public static User InvalidUser1 = new User
        {
            UserId = 298,
            Name = "MsPassenger",
            Password = "!",
            UserType = UserType.Passenger
        };

    }
}
