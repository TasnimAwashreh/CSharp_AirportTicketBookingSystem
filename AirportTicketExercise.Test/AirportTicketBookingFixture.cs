using AirportTicketBookingExercise.App.Commands.Helpers;
using AirportTicketBookingExercise.App.Configuration;
using AirportTicketBookingExercise.Logic.Utils;
using ATB.App;
using ATB.App.Configuration;
using ATB.Data.Db;
using ATB.Data.Models;
using ATB.Logic.Service;
using CsvHelper;
using Microsoft.Extensions.DependencyInjection;

namespace AirportTicketExercise.Test
{
    public class AirportTicketBookingFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }
        public IBookingService BookingService { get; }
        public IFlightService FlightService { get; }
        public IUserService UserService { get; }

        private DatabaseManager _databaseManager { get; }

        public AirportTicketBookingFixture()
        {
            var services = new ServiceCollection();
            services
                .AddRepositories(Constants.TestUsersPath, Constants.TestFlightsPath, Constants.TestBookingsPath)
                .AddServices();

            ServiceProvider = services.BuildServiceProvider();
            using var scope = ServiceProvider.CreateScope();
            _databaseManager = scope.ServiceProvider.GetRequiredService<DatabaseManager>();
            _databaseManager.CreateDatabase();

            FlightService = scope.ServiceProvider.GetRequiredService<IFlightService>();
            UserService = scope.ServiceProvider.GetRequiredService<IUserService>();
            BookingService = scope.ServiceProvider.GetRequiredService<IBookingService>();

            ResetFiles();
        }

        public void ResetFiles()
        {
            File.Delete(Constants.TestBookingsPath);
            File.Delete(Constants.TestFlightsPath);
            File.Delete(Constants.TestUsersPath);

            _databaseManager.CreateDatabase();
        }

        public void Dispose()
        {
            ServiceProvider.Dispose();
        }

    }
}
