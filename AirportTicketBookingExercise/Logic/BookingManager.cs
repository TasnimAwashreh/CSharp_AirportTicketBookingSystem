using ATB.Logic.Handlers.Command;
using ATB.Logic.Enums;

namespace ATB.Logic
{
    public class BookingManager
    {

        private PassengerCommandHandler _passengerCommandHandler;
        private ManagerCommandHandler _managerCommandHandler;

        public BookingManager(
            ManagerCommandHandler managerCommndHandler, PassengerCommandHandler passengerCommandHandler)
        {
            _passengerCommandHandler = passengerCommandHandler;
            _managerCommandHandler = managerCommndHandler;
        }

        public void ProcessInput(string input)
        {
            string[] line = input.Split(' ');
            ManagerCommand managerCommand = line[0].ParseManagerCommand();
            PassengerCommand passengerCommand = line[0].ParsePassengerCommand();
            if (passengerCommand != PassengerCommand.None)
                _passengerCommandHandler.ExecutePassengerCommand(line, passengerCommand);
            else if (managerCommand != ManagerCommand.None)
                _managerCommandHandler.executeManagerCommand(line, managerCommand);
            else Console.WriteLine("\n Please enter an appropriate action");
        }


    }

}

