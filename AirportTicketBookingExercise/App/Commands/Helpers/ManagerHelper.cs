using ATB.Data.Models;
using ATB.Logic.Service;
using System.ComponentModel.DataAnnotations;
using System.Data;

namespace AirportTicketBookingExercise.App.Commands.Helpers
{
    public class ManagerHelper
    {
        private readonly IFlightService _flightService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        public ManagerHelper(IFlightService flightService, IUserService userService, IBookingService bookingService)
        {
            _flightService = flightService;
            _userService = userService;
            _bookingService = bookingService;
        }

        public User? ManagerSignUp(string[] productInfo)
        {
            try
            {
                if (productInfo.Length < 3)
                    throw new FormatException();

                var user = new User
                {
                    Name = productInfo[1],
                    Password = productInfo[2],
                    UserType = UserType.Manager
                };

                _userService.CreateUser(user);
                var newUser = _userService.GetUserByName(productInfo[1]);
                if (newUser == null) Console.WriteLine("User has not been added, please try again another time");
                else Console.WriteLine("You have signed up successfully, manager");
                return newUser;
            }
            catch(FormatException)
            {
                Console.WriteLine("Please enter a username and password");
                return null;
            }
            catch (DuplicateNameException)
            {
                Console.WriteLine("This user already exists!");
                return null;
            }
            catch (ValidationException)
            {
                Console.WriteLine("Username and Password must be in between 3 and 12");
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public User? ManagerLogin(string[] productInfo)
        {
            if (productInfo.Length < 3)
            {
                Console.WriteLine("Please enter your username and password to login, manager!");
                return null;
            }

            try
            {
                var user = new User
                {
                    Name = productInfo[1],
                    Password = productInfo[2],
                    UserType = UserType.Manager
                };

                User? loggingInUser = _userService.Authenticate(user);
                if (loggingInUser == null)
                {
                    Console.WriteLine("Incorrect username or password, please try again");
                    return null;
                }

                Console.WriteLine($"Welcome back, {loggingInUser.Name}!");
                return loggingInUser;
            }
            catch (KeyNotFoundException ex) 
            {
                Console.WriteLine("Username and Password must be in between 3 and 12");
                return null;
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
                return null;
            }
        }

        public void Upload(string[] productInfo)
        {
            if (productInfo.Length < 2)
            {
                Console.WriteLine("Please enter a valid CSV file path");
                return;
            }
            if (!File.Exists(productInfo[1]))
            {
                Console.WriteLine("This file does not exist");
                return;
            }

            try
            {
                _flightService.ImportFlightData(productInfo[1]);
                Console.WriteLine("Imported successfully");
            }
            catch(FormatException ex)
            {
                Console.WriteLine("Please use the Validate command before importing as this file contains invalid fields \n");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Upload did not succeed. Please try again later. \n {ex}");
            }
        }

        public void ValidateCSV(string[] productInfo)
        {
            Console.WriteLine($"\n Validating... \n");
            if (productInfo.Length < 2)
            {
                Console.WriteLine("Please enter a valid CSV file path");
                return;
            }
            if (!File.Exists(productInfo[1]))
            {
                Console.WriteLine("This file does not exist");
                return;
            }
            string errorStr = _flightService.ValidateFlightData(productInfo[1]);
            if (errorStr != "") Console.WriteLine(errorStr);
            else Console.WriteLine("No fields are in need of fixing!");
        }

        public void Filter(string[] productInfo)
        {
            Console.WriteLine($"\n Filter: \n");
            List<Booking> bookingResults = _bookingService.FilterBookings(productInfo);
            if (bookingResults.Count > 0)
            {
                Console.WriteLine("\nFilter Results: \n");
                foreach (Booking Booking in bookingResults)
                {
                    Console.WriteLine(Booking.ToString() + $" Booked by passenger with Id ({Booking.PassengerId})");
                }
            }
        }
    }
}
