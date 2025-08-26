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

        public bool CreateUser(User user)
        {
            try
            {
                using (var writer = new StreamWriter(_usersPath, append: true))
                using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
                {
                    csv.Context.RegisterClassMap<UserMap>();
                    csv.WriteRecord<User>(user);
                    csv.NextRecord();
                    return true;
                }
            }
            catch
            {
                return false;
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
