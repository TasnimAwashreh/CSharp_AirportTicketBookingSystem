using ATB.Data.Models;

public interface IUserService
{
    public void CreateUser(User user);
    public void UpdateUser(User user);
    public User? GetUser(int userId);
    public User? GetUserByName(string username);
    public User? Authenticate(User user);
    public List<User> GetUserByType(UserType type);

}
