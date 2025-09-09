using ATB.Data.Models;

public interface IUserService
{
    public void CreateUser(string name, string password, UserType usertype);
    public void UpdateUser(User user);
    public User? GetUser(int userId);
    public User? GetUserByName(string username);
    public User? Authenticate(string username, string password, UserType usertype);
    public List<User> GetUserByType(UserType type);

}
