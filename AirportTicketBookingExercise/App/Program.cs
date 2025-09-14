using ATB.App.Configuration;
using ATB.App;
using ATB.Data.Db;
using Microsoft.Extensions.DependencyInjection;
using AirportTicketBookingExercise.App.Configuration;

class Program
{
    public static void StartLoop(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();
        var databaseManager = scope.ServiceProvider.GetRequiredService<DatabaseManager>();
        databaseManager.CreateDatabase();
        var BookingManager = scope.ServiceProvider.GetRequiredService<BookingManager>();

        while (true)
        {
            Console.WriteLine("--------------------------------------------------------------------------------------------");
            var input = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(input))
                Console.WriteLine("Empty input: Please try again");
            else
            {
                string[] lines = input.Split(' ');
                if (lines.Length == 0)
                {
                    Console.WriteLine("Please enter a command ");
                    continue;
                }
                BookingManager.ProcessInput(lines);
            }
            
        }
    }

    public static string Introduction()
    {
        string controls =
            $"""
            ============================================================================================
            **                                                                                        **
            **                           Welcome to the Airport Ticket Booking System                 **
            **                                                                                        **
            ============================================================================================

            """;
        return controls;
    }

    public static void Main(string[] args)
    {
        var services = new ServiceCollection();
        services
            .AddRepositories(Constants.UsersPath, Constants.FlightsPath, Constants.BookingsPath)
            .AddServices()
            .AddManager();

        Console.WriteLine(Introduction());
        var serviceProvider = services.BuildServiceProvider();
        StartLoop(serviceProvider);
    }
}
