namespace ATB.Logic.Enums
{
    public enum ManagerCommand
    {
        manager_login,
        manager_signup,
        manager_logout,
        validate,
        upload,
        filter,
        none
    }

    public class ManagerCommands
    {
        public static ManagerCommand GetManagerCommand(string command)
        {
            switch (command.ToLower())
            {
                case "manager_signup":
                    return ManagerCommand.manager_signup;
                case "manager_login":
                    return ManagerCommand.manager_login;
                case "manager_logout":
                    return ManagerCommand.manager_logout;
                case "validate":
                    return ManagerCommand.validate;
                case "upload":
                    return ManagerCommand.upload;
                case "filter":
                    return ManagerCommand.filter;
                default:
                    return ManagerCommand.none;
            }
        }

    }
}
