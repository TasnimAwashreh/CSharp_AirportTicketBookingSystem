using ATB.Data.Db.DAOs;
using ATB.Data.Models;


namespace ATB.Data.Repository
{
    public class UserRepository : IUserRepository
    {
       private readonly UserDAO _dao;

        public UserRepository(UserDAO dao) { 
            _dao = dao;
        }
        public bool CreateUser(User User)
        {
            try
            {
                _dao.CreateUser(User);
                return true;
            }
            catch
            {
                return false;
            }

        }

        public List<User> GetAllUsers() {

            return _dao.GetAllUsers();
        }
        public User? GetUserById(int id) => _dao.GetUserById(id);

        public User? GetUserByName(string name) => _dao.GetUserByUsername(name);

        public List<User> GetUserByType(UserType type)
        {
            return _dao.GetUsersByType(type);
        }

        public bool UpdateUser(User User)
        {
            throw new NotImplementedException();
        }
    }
}
