using ATB.Data.Models;
using ATB.Logic.Enums;
using ATB.Logic.Service;


namespace ATB.Logic.Handlers.Command
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

        public bool Upload(string flightCSVPath)
        {
            return _flightService.ImportFlightData(flightCSVPath);
        }

        public string Validate(string flightCSVPath)
        {
            return _flightService.ValidateFlightData(flightCSVPath);
        }

        public List<Booking> Filter(string[] input)
        {
            BookingFilter Filter = BookingFilters.Parse(input.Skip(1).ToArray());
            List<Booking> bookingResults = new List<Booking>();
            bookingResults = _bookingService.FilterBookings(Filter);
            return bookingResults;
        }

        public User? ManagerLogIn(string[] productInfo, User? loggedInUser)
        {
            if (productInfo.Length < 3)
                return null;
            string username = productInfo[1];
            string password = productInfo[2];

            var user = _userService.Authenticate(username, password);
            if (user == null || user.UserType != UserType.Manager)
                return null;
            return user;
        }

        public bool ManagerSignUp(string[] productInfo)
        {
            if (productInfo.Length < 3)
                return false;
            try
            {
                var user = new User
                {
                    UserId = new Random().Next(100000, 999999),
                    Name = productInfo[1],
                    Password = productInfo[2],
                    UserType = UserType.Manager
                };

                if (_userService.GetUserByName(user.Name) != null)
                    return false;
                return _userService.CreateUser(user);
            }
            catch
            {
                return false;
            }
            
        }
    }

}
