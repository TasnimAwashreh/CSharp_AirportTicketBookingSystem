using AirportTicketBookingExercise.App.Commands.CommandExecuter;
using AirportTicketBookingExercise.App.Commands.Enums;
using ATB.Data.Models;

namespace ATB.App
{
    public class BookingManager
    {
        private ICommandExecuter<ManagerCommand> _managerExecuter;
        private ICommandExecuter<PassengerCommand> _passengerExecuter;
        private User? _loggedInUser;

        public BookingManager(ICommandExecuter<ManagerCommand> managerExecuter, ICommandExecuter<PassengerCommand> passengerExecuter)
        {
            _managerExecuter = managerExecuter;
            _passengerExecuter = passengerExecuter;
        }

        public void ProcessInput(string[] input)
        {
            try
            {
                ManagerCommand managerCommand = input[0].ParseManagerCommand();
                PassengerCommand passengerCommand = input[0].ParsePassengerCommand();
                if (passengerCommand != PassengerCommand.None)
                    _passengerExecuter.ExecuteCommand(_loggedInUser, input, passengerCommand);
                else if (managerCommand != ManagerCommand.None)
                    _managerExecuter.ExecuteCommand(_loggedInUser, input, managerCommand);
                else Console.WriteLine("\n Please enter an appropriate action");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing your input: {ex.Message}");
            }
        }
    }
}

