using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IUserRepository
    {
        public bool CreateUser(User User);
        public bool UpdateUser(User User);
        public List<User> GetAllUsers();
        public List<User> GetUsersByType(UserType type);
        public User? GetUser(int userId);
        public User? GetUser(string username);
        public bool RemoveUser(User User);
    }
}