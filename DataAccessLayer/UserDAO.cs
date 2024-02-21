using BusinessObject;

namespace DataAccessLayer
{
    public class UserDAO : BaseDAO<User>
    {
        private UserDAO() { }

        private static UserDAO instance = null;
        private static readonly object instacelock = new object();

        public static UserDAO Instance
        {
            get
            {
                lock (instacelock)
                {
                    if (instance == null)
                    {
                        instance = new UserDAO();
                    }
                    return instance;
                }
            }
        }

        public bool IsUsernameDuplicate(string username)
        {
            try
            {
                return GetAll().Any(c => c.UserName.Equals(username, StringComparison.OrdinalIgnoreCase));
            }
            catch (Exception ex)
            {
                return false;
            }

        }

        public User GetUserById(int userId)
        {
            return GetAll().FirstOrDefault(u => u.UserId == userId);
        }
    }
}
