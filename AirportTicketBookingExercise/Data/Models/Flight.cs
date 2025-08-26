
using ATB.Data.Attributes;
using System.ComponentModel.DataAnnotations;

namespace ATB.Data.Models
{
    public class Flight
    {
        [Required(ErrorMessage = "Flight Id is missing")]
        public int FlightId { get; set; }

        [Required(ErrorMessage = "Flight Name is missing")]
        [MaxLength(35, ErrorMessage = "Flight Name must be shorter than 35 characters")]
        [MinLength(3, ErrorMessage = "Flight Name must be longer than 3 characters")]
        public string FlightName { get; set; }

        [Required(ErrorMessage = "Departure Country is missing")]
        [MaxLength(35, ErrorMessage = "Departure Country must be shorter than 35 characters")]
        [MinLength(3, ErrorMessage = "Departure Country must be longer than 3 characters")]
        public string DepartureCountry { get; set; }

        [Required(ErrorMessage = "Destination Country is missing")]
        [MaxLength(35, ErrorMessage = "Destination Country must be shorter than 35 characters")]
        [MinLength(3, ErrorMessage = "Destination Country must be longer than 3 characters")]
        public string DestinationCountry { get; set; }

        [Required(ErrorMessage = "Depature Airport is missing")]
        [MaxLength(50, ErrorMessage = "Depature Airport must be shorter than 50 characters")]
        [MinLength(3, ErrorMessage = "Depature Airport must be longer than 3 characters")]
        public string DepartureAirport { get; set; }

        [Required(ErrorMessage = "Arrival Airport is missing")]
        [MaxLength(50, ErrorMessage = "Arrival Airport must be shorter than 50 characters")]
        [MinLength(3, ErrorMessage = "Arrival Airport must be longer than 3 characters")]
        public string ArrivalAirport { get; set; }

        [PresentAndFutureOnly]
        [Required(ErrorMessage = "Depature Date is missing")]
        public DateTime DepartureDate { get; set; }

        [Required(ErrorMessage = "Economy Seat Price is missing")]
        public decimal EconomyPrice { get; set; }

        [Required(ErrorMessage = "Business Class Price is missing")]
        public decimal BuisnessPrice { get; set; }

        [Required(ErrorMessage = "First Class Price is missing")]
        public decimal FirstClassPrice { get; set; }

        [Required(ErrorMessage = "Seat Capacity is missing")]
        public int SeatCapacity { get; set; }

        public int SeatsAvailable { get; set; }

        public override string ToString()
        {
            return $"{this.FlightId}| {this.FlightName}: Leaving on {this.DepartureDate} from {this.DepartureCountry}, {this.DepartureAirport} -> {this.DestinationCountry}, {this.DepartureAirport}." +
                $"\n {this.FlightName} Prices: " +
                (this.EconomyPrice != 0 ? ("Economy Price: " + this.EconomyPrice.ToString()) : "") +
                (this.BuisnessPrice != 0 ? (" Business Class Price: " + this.BuisnessPrice.ToString()) : "") +
                (this.FirstClassPrice != 0 ? (" First Class Price: " + this.FirstClassPrice.ToString()) : "");
        }
    }
}
