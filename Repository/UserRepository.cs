using BusinessObject;
using DataAccessLayer;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<User> GetUsers()
        {
            return UserDAO.Instance.GetAll();
        }
    }
}
