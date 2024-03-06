using BusinessObject;
using DataAccessLayer;
using Repository.DTOs;

namespace Repository
{
    public class UserRepository : IUserRepository
    {
        public PaginatedList<UserDto> GetUsers(string? searchString, int? pageIndex, int pageSize, int userId)
        {
            var users = UserDAO.Instance.GetUsers();

            if (userId != 0)
            {
                users = users.Where(u => u.UserId != userId);
            }

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(u => u.UserName.ToLower().Contains(searchString.ToLower()));
            }

            var userDtos = users.Select(user => new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                Introduction = user.Introduction,
                Interest = user.Interest,
                City = user.City,
                Avatar = user.photos.FirstOrDefault(p => p.isMain)?.PhotoUrl
            }).ToList();


            return PaginatedList<UserDto>.CreateAsync(
                userDtos.AsQueryable(), pageIndex ?? 1, pageSize);
        }

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

        public bool IsUserNameValidForUpdate(int userId, string username)
        {
            return UserDAO.Instance.IsUserNameValidForUpdate(userId, username);
        }

        public List<UserDto> GetUserInGroupChat(int conversationId)
        {
            List<User> users = UserDAO.Instance.GetUserInGroupChat(conversationId);
            List<UserDto> usersDto = new List<UserDto>();
            foreach (User user in users)
            {

                UserDto userDto = new UserDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    DateOfBirth = user.DateOfBirth,
                    KnownAs = user.KnownAs,
                    Gender = user.Gender,
                    Introduction = user.Introduction,
                    Interest = user.Interest,
                    City = user.City,
                    Avatar = user.photos.FirstOrDefault(p => p.isMain)?.PhotoUrl
                };
                if (!usersDto.Contains(userDto))
                {
                    usersDto.Add(userDto);
                }
            }

            return usersDto;
        }

        public UserDto GetUserDtoWithPhoto(int userId)
        {
            User user = UserDAO.Instance.GetUser(userId);

            return new UserDto
            {
                UserId = user.UserId,
                UserName = user.UserName,
                DateOfBirth = user.DateOfBirth,
                KnownAs = user.KnownAs,
                Gender = user.Gender,
                Introduction = user.Introduction,
                Interest = user.Interest,
                City = user.City,
                Avatar = user.photos.FirstOrDefault(p => p.isMain)?.PhotoUrl
            };
        }

        public bool CheckFriendUser(int userId, int otherUserId)
        {
            List<Friend> friendList = FriendDAO.Instance.GetFriendsForUser(userId);

            var friend = friendList.FirstOrDefault(l => l.RecipientId == otherUserId);

            if (friend != null)
            {
                return true;
            }
            return false;
        }

    }
}

