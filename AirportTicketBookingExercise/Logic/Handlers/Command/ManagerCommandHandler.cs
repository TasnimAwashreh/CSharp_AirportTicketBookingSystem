using ATB.Logic.Enums;
using ATB.Data.Models;
using ATB.Logic.Service;
using ATB.Logic;


namespace ATB.Logic.Handlers.Command
{
    public class ManagerCommandHandler
    {
       
        private readonly IFlightservice _flightService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        public ManagerCommandHandler(IFlightservice flightService,IUserService userService, IBookingService bookingService)
        {
            _flightService = flightService;
            _userService = userService;
            _bookingService = bookingService;
              
        }

        public bool Upload()
        {
            return _flightService.ImportFlightData();
        }
        public string Validate()
        {
            Console.WriteLine($"\n Validating... \n");
            return _flightService.ValidateFlightData();
        }
        public List<Booking> Filter(string[] input)
        {
            BookingFilter Filter = BookingFilters.Parse(input.Skip(1).ToArray());
            List<Booking> bookingResults = new List<Booking>();
            bookingResults = _bookingService.FilterBookings(Filter);
            return bookingResults;  
        }

        public bool ManagerLogIn(string[] productInfo, User? loggedInUser)
        {
            if (productInfo.Length >= 3)
            {
                string username = productInfo[1];
                string password = productInfo[2];

                var user = _userService.Authenticate(username, password);

                if (user != null && user.UserType == UserType.Manager)
                {
                    loggedInUser = user;
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public bool ManagerSignOut(User? loggedInUser)
        {
            if (loggedInUser == null || loggedInUser.UserType != UserType.Manager)
                return false;
            else
            {
                loggedInUser = null;
                return true;
            }
        }

        public bool ManagerSignUp(string[] productInfo)
        {
            if (productInfo.Length < 3)
                return false;
            else
            {
                try
                {
                    var user = new User
                    {
                        Name = productInfo[1],
                        Password = productInfo[2],
                        UserType = UserType.Manager
                    };

                    if (_userService.GetUserByName(user.Name) != null)
                        return false;
                    else return _userService.CreateUser(user);
                }
                catch
                {
                    return false;
                }
            }
        }
    }

}
