using BusinessObject;

namespace Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();

        void CreateUser(User user);

        bool IsUsernameDuplicate(string username);
    }
}
