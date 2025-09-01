using AirportTicketBookingExercise.Logic.Utils;
using ATB.Data.Extensions;
using ATB.Data.Models;
using CsvHelper;
using System.Globalization;

namespace ATB.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly string _usersPath;

        public UserRepository(string usersPath)
        {
            _usersPath = usersPath;
        }

        public List<User> GetAllUsers()
        {
            return CsvActionsHelper.GetAllRecords<User, UserMap>(_usersPath);
        }

        public List<User> GetUsersByType(UserType type)
        {
            return GetAllUsers()
                    .Where(u => u.UserType == type).ToList();
        }

        public User? GetUser(string username)
        {
            return GetAllUsers()
                    .FirstOrDefault(u => u.Name.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        public User? GetUser(int userId)
        {
            return GetAllUsers()
                    .FirstOrDefault(u => u.UserId == userId);
        }

        public void CreateUser(User user)
        {
            user.GenerateUserId();
            CsvActionsHelper.CreateRecord<User, UserMap>(_usersPath, user);
            
        }

        public bool UpdateUser(User User)
        {
            throw new NotImplementedException();
        }

        public bool RemoveUser(User User)
        {
            throw new NotImplementedException();
        }
    }
}
