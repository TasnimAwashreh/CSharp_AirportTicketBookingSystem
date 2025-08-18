using ATB.Data.Db.DAOs;
using ATB.Data.Repository;
using ATB.Logic.Handlers.Command;
using ATB.Logic.Service;
using ATB.Logic;
using Microsoft.Extensions.DependencyInjection;


namespace ATB.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddDaos(this IServiceCollection services){

            const string DbFile = "bookingDb.db";
            string connectionString = Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", "Data", "Db", DbFile);

            services
                    .AddSingleton<UserDAO>(_ => new UserDAO(connectionString))
                     .AddSingleton<BookingDAO>(_ => new BookingDAO(connectionString))
                     .AddSingleton<DatabaseManager>(_ => new DatabaseManager(connectionString));

            return services;
        }


        public static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services
                .AddScoped<IUserRepository, UserRepository>()
                .AddScoped<IFlightRepository, FlightRepository>()
                .AddScoped<IBookingRepository, BookingRepository>();

            return services;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services
                .AddScoped<IUserService, UserService>()
                .AddScoped<IFlightService, FlightService>()
                .AddScoped<IBookingService, BookingService>();
            return services;
        }

        public static IServiceCollection AddCommandHandlers(this IServiceCollection services)
        {
            services
                .AddScoped<PassengerCommandHandler>()
                .AddScoped<ManagerCommandHandler>();

            return services;
        }
        public static IServiceCollection AddManager(this IServiceCollection services)
        {
            services.AddScoped<BookingManager>();
            return services;
        }

    }
}
