using BusinessObject;

namespace Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        void CreateUser(User user);

        bool IsUsernameDuplicate(string username);

        void UpdateUser(User user);
        User GetUser(int userId);

        User Login(string username, string password);
        User GetUserWithPhoto(int  userId);
    }
}
