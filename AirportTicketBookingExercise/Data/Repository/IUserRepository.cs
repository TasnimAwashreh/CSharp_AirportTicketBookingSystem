using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IUserRepository
    {

        public bool CreateUser(User User);
        public bool UpdateUser(User User);
        public User? GetUser(int id);
        public User? GetUserByName(string username);
        public List<User> GetUserByType(UserType type);
        public List<User> GetAllUsers();
    }

}