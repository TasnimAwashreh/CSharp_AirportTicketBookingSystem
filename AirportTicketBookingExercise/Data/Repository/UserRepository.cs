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
            using (var reader = new StreamReader(_usersPath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<UserMap>();
                var records = csv.GetRecords<User>().ToList();
                return records;
            }
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
            using (var writer = new StreamWriter(_usersPath, append: true))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.Context.RegisterClassMap<UserMap>();
                csv.WriteRecord<User>(user);
                csv.NextRecord();
            }
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
