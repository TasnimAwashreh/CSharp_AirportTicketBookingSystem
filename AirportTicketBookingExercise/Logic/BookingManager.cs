using ATB.Logic.Handlers.Command;
using ATB.Logic.Enums;

namespace ATB.Logic
{
    public class BookingManager
    {

        private PassengerCommandHandler _passengerCommandHandler;
        private ManagerCommandHandler _managerCommandHnadler;

        public BookingManager(
            ManagerCommandHandler managerCommndHandler, PassengerCommandHandler passengerCommandHandler)
        {
            _passengerCommandHandler = passengerCommandHandler;
            _managerCommandHnadler = managerCommndHandler;
        }

        public void processInput(string input)
        {
            string[] line = input.Split(' ');
            ManagerCommand managerCommand = ManagerCommands.GetManagerCommand(line[0]);
            PassengerCommand passengerCommand = PassengerCommands.GetPassengerCommand(line[0]);
            if (passengerCommand != PassengerCommand.none)
                _passengerCommandHandler.ExecutePassengerCommand(line, passengerCommand);
            else if (managerCommand != ManagerCommand.none)
                _managerCommandHnadler.executeManagerCommand(line, managerCommand);
            else Console.WriteLine("\n Please enter an appropriate action");
        }


    }

}

