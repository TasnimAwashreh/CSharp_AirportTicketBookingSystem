using ATB.Logic.Enums;
using CsvHelper.Configuration;

namespace ATB.Data.Models
{
    public class Booking
    {
        public int BookingId;
        public int FlightId;
        public int PassengerId;
        public BookingClass BookingClass;

        public override string ToString()
        {
            return $"{BookingId}| Flight Id: {this.FlightId}, Class: {this.BookingClass}, Passenger Id: {this.PassengerId}";
        }

    }

    public sealed class BookingMap : ClassMap<Booking>
    {
        public BookingMap()
        {
            Map(m => m.BookingId).Name("BookingId");
            Map(m => m.FlightId).Name("FlightId");
            Map(m => m.PassengerId).Name("PassengerId");
            Map(m => m.BookingClass).Name("BookingClass");
        }
    }


}
