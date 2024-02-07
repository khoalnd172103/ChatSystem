using BusinessObject;

namespace Repository
{
    public interface IUserRepository
    {
        IEnumerable<User> GetUsers();
    }
}
