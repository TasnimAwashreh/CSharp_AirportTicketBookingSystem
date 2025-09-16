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
    public class UserServiceFixture : IDisposable
    {
        public ServiceProvider ServiceProvider { get; }
        public IUserService UserService { get; }

        private DatabaseManager _databaseManager { get; }

        public UserServiceFixture()
        {
            var services = new ServiceCollection();
            services
                .AddRepositories(Constants.TestUsersPath, Constants.TestFlightsPath, Constants.TestBookingsPath)
                .AddServices();

            ServiceProvider = services.BuildServiceProvider();
            using var scope = ServiceProvider.CreateScope();
            _databaseManager = scope.ServiceProvider.GetRequiredService<DatabaseManager>();
            _databaseManager.CreateDatabase();
            UserService = scope.ServiceProvider.GetRequiredService<IUserService>();

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
