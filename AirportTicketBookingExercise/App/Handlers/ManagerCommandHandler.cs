using ATB.Data.Models;
using ATB.Logic;
using ATB.Logic.Enums;
using ATB.Logic.Service;

namespace ATB.App.Handlers
{
    public class ManagerCommandHandler
    {

        private readonly IFlightservice _flightService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        public ManagerCommandHandler(IFlightservice flightService, IUserService userService, IBookingService bookingService)
        {
            _flightService = flightService;
            _userService = userService;
            _bookingService = bookingService;
        }

        public string Upload(string flightCSVPath)
        {
            if (!File.Exists(flightCSVPath))
                return "This file does not exist";
            bool isSuccessful = _flightService.ImportFlightData(flightCSVPath);
            if (!isSuccessful)
                return "Please use the Validate command before importing as this file contains invalid fields";
            else return "Imported successfully";
        }

        public string Validate(string flightCSVPath)
        {
            if (!File.Exists(flightCSVPath))
                return "No file exists in this path";
            return _flightService.ValidateFlightData(flightCSVPath);
        }

        public List<Booking> Filter(string[] filterInput)
        {
            BookingFilter Filter = BookingFilters.Parse(filterInput.Skip(1).ToArray());
            List<Booking> bookingResults = new List<Booking>();
            bookingResults = _bookingService.FilterBookings(Filter);
            return bookingResults;
        }

        public User? ManagerLogIn(string name, string pass)
        {
            string username = name;
            string password = pass;

            var user = _userService.Authenticate(username, password);
            if (user == null || user.UserType != UserType.Manager)
                return null;
            return user;
        }

        public bool ManagerSignUp(string name, string password)
        {
            try
            {
                var user = new User
                {
                    Name = name,
                    Password = password,
                    UserType = UserType.Manager
                };
                if (_userService.GetUserByName(user.Name) != null)
                    return false;
                return _userService.CreateUser(user);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nError when creating a new Manager: {ex.ToString()}");
                return false;
            }
        }
    }
}
