using ATB.Data.Models;

public interface IUserService
{
    bool CreateUser(User user);
    bool UpdateUser(User user);
    User? GetUserById(int userId);
    User? GetUserByName(string username);
    User? Authenticate(string username, string password);
    public List<User> GetUserByType(UserType type);

}
