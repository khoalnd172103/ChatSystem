using BusinessObject;
using DataAccessLayer;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<User> GetUsers() => UserDAO.Instance.GetAll();

        public void CreateUser(User user)
        {
            UserDAO.Instance.Create(user);
        }

        public bool IsUsernameDuplicate(string username)
        {
            return UserDAO.Instance.IsUsernameDuplicate(username);
        }
    }
}
