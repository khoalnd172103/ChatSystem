using BusinessObject;
using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;

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

        public IEnumerable<User> GetUsers()
        {
            var db = new DataContext();
            return db.Users.Include(u => u.photos).ToList();
        }

        public User GetUser(int userId)
        {
            var db = new DataContext();
            return db.Users.Include(c => c.photos).SingleOrDefault(u => u.UserId.Equals(userId));
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

        public bool IsUserNameValidForUpdate(int userId, string username)
        {
            try
            {
                return GetAll().Any(u => u.UserName.Equals(username) && u.UserId != userId);
            }
            catch (Exception ex)
            {
                return false;
            }
        }
    }
}
