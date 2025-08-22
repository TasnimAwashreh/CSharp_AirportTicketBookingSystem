using ATB.Data;
using ATB.Logic.Enums;
using ATB.Data.Models;
using ATB.Logic.Service;


namespace ATB.Logic.Handlers.Command
{
    public class ManagerCommandHandler
    {
       
        private readonly IFlightservice _flightService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;
        private User? loggedInUser;

        public ManagerCommandHandler(IFlightservice flightService,IUserService userService, IBookingService BookingService)
        {
            _flightService = flightService;
            _userService = userService;
            _bookingService = BookingService;
              
        }
        public void executeManagerCommand(string[] productInfo, ManagerCommand command)
        {
            if (loggedInUser != null)
            {
                switch (command)
                {
                    case ManagerCommand.ManagerLogOut:
                        ManagerSignOut();
                        break;
                    case ManagerCommand.Upload:
                        Upload();
                        break;
                    case ManagerCommand.Validate:
                        Validate();
                        break;
                    case ManagerCommand.Filter:
                        Console.WriteLine($"\n Filter: \n");
                        Filter(productInfo);
                        break;
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        break;
                }
            }
            else
            {
                switch (command)
                {
                    case ManagerCommand.ManagerSignUp:
                        ManagerSignUp(productInfo);
                        break;
                    case ManagerCommand.ManagerLogIn:
                        ManagerLogIn(productInfo);
                        break;
                    case ManagerCommand.None:
                        Console.WriteLine("\n Manager, please enter an appropriate action");
                        break;
                    default:
                        Console.WriteLine("You must log in to use this command");
                        break;
                }
            }
        }

        public void Upload()
        {
            Console.WriteLine($"\n Uploading... \n");
            bool isSuccess = _flightService.ImportFlightData();
            if (isSuccess == true)
                Console.WriteLine("Flight Data has been imported successfully");
            else Console.WriteLine("The CSV is not in the correct format. " +
                "Please use the 'Validate' command to check which fields must be changed.");
        }
        public void Validate()
        {
            Console.WriteLine($"\n Validating... \n");
            string errorStr = _flightService.ValidateFlightData();
            if (errorStr != "")
                Console.WriteLine(errorStr);
            else Console.WriteLine("No fields are in need of fixing!");
        }
        public void Filter(string[] input)
        {
            BookingFilter Filter = BookingFilters.Parse(input.Skip(1).ToArray());
            List<Booking> BookingResults = _bookingService.FilterBookings(Filter);
            if (BookingResults.Count > 0)
            {
                Console.WriteLine("Filter Results: \n");
                foreach (Booking Booking in BookingResults)
                {
                    Console.WriteLine(Booking.ToString() + $" Booked by passenger with Id ({Booking.PassengerId})");
                }
            }
        }

        private void ManagerLogIn(string[] productInfo)
        {
            if (productInfo.Length >= 3)
            {
                string username = productInfo[1];
                string password = productInfo[2];

                var user = _userService.Authenticate(username, password);

                if (user != null && user.UserType == UserType.Manager)
                {
                    loggedInUser = user;
                    Console.WriteLine($"Welcome back, {user.Name}!");
                }
                else Console.WriteLine("Invalid credentials or not a manager.");
            }
            else Console.WriteLine("Please enter your name and password.");
        }

        private void ManagerSignOut()
        {
            if (loggedInUser != null)
            {
                Console.WriteLine($"Manager {loggedInUser.Name} has logged out.");
                loggedInUser = null;
            }
            else Console.WriteLine("No Manager is currently logged in.");
        }

        private void ManagerSignUp(string[] productInfo)
        {
            if (productInfo.Length >= 3)
            {
                try
                {
                    var user = new User
                    {
                        Name = productInfo[1],
                        Password = productInfo[2],
                        UserType = UserType.Manager
                    };

                    if (_userService.GetUserByName(user.Name) == null)
                    {
                        bool isSuccess = _userService.CreateUser(user);
                        if (isSuccess)
                            Console.WriteLine($"Manager {user.Name} signed up successfully.");
                        else
                            Console.WriteLine("Manager SignUp was unsuccessful.");
                    }
                    else Console.WriteLine("Manager with this username already exists.");
                }
                catch
                {
                    Console.WriteLine("Something went wrong. Please try again later.");
                }
            }
            else
            {
                Console.WriteLine("Please enter name and password to sign up as a manager.");
            }
        }
    }

}
