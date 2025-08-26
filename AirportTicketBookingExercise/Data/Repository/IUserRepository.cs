using ATB.Data.Models;

namespace ATB.Data.Repository
{
    public interface IUserRepository
    {
        public bool CreateUser(User User);

        public bool UpdateUser(User User);

        public List<User> GetAllUsers();

        public bool RemoveUser(User User);
    }
}