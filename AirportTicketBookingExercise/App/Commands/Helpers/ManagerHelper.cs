using ATB.Data.Models;
using ATB.Logic.Service;
using System.ComponentModel.DataAnnotations;

namespace AirportTicketBookingExercise.App.Commands.Helpers
{
    public class ManagerHelper
    {
        private readonly IFlightservice _flightService;
        private readonly IUserService _userService;
        private readonly IBookingService _bookingService;

        public ManagerHelper(IFlightservice flightService, IUserService userService, IBookingService bookingService)
        {
            _flightService = flightService;
            _userService = userService;
            _bookingService = bookingService;
        }

        public void ManagerSignUp(string[] productInfo)
        {
            if (productInfo.Length < 3)
            {
                Console.WriteLine("Please enter a username and password");
                return;
            }

            try
            {
                _userService.CreateUser(productInfo[1], productInfo[2], UserType.Manager);
                Console.WriteLine("You have signed up successfully, manager");
            }
            catch (ValidationException ex)
            {
                Console.WriteLine("Username and Password must be in between 3 and 12");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
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
                User? loggingInUser = _userService.Authenticate(productInfo[1], productInfo[2], UserType.Manager);
                if (loggingInUser == null)
                {
                    Console.WriteLine("Incorrect username or password, please try again");
                    return null;
                }

                Console.WriteLine($"Welcome back, {loggingInUser.Name}!");
                return loggingInUser;
            }
            catch (ValidationException ex) 
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
            bool isSuccessful = _flightService.ImportFlightData(productInfo[1]);
            if (!isSuccessful) Console.WriteLine("Please use the Validate command before importing as this file contains invalid fields");
            else Console.WriteLine("Imported successfully");
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
