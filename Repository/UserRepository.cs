using BusinessObject;
using DataAccessLayer;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public IEnumerable<User> GetUsers() => UserDAO.Instance.GetUsers();

        public void CreateUser(User user)
        {
            user.UserPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(user.UserPassword, 10);
            UserDAO.Instance.Create(user);
        }

        public bool IsUsernameDuplicate(string username)
        {
            return UserDAO.Instance.IsUsernameDuplicate(username);
        }

        public void UpdateUser(User user) => UserDAO.Instance.Update(user);

        public User GetUser(int userId)
        {
            return UserDAO.Instance.GetUserById(userId);
        }

        public User Login(string username, string password) => UserDAO.Instance.GetAll().FirstOrDefault(p => p.UserName.Equals(username) && BCrypt.Net.BCrypt.EnhancedVerify(password, p.UserPassword), null);

        public User GetUserWithPhoto(int userId)
        {
            return UserDAO.Instance.GetUser(userId);
        }
    }
}
