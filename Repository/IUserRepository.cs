using BusinessObject;
using Repository.DTOs;

namespace Repository
{
    public interface IUserRepository
    {
        PaginatedList<UserDto> GetUsers(string? searchString, int? pageIndex, int pageSize, int userId);

        void CreateUser(User user);

        bool IsUsernameDuplicate(string username);

        void UpdateUser(User user);
        User GetUser(int userId);

        User Login(string username, string password);
        User GetUserWithPhoto(int userId);
        bool IsUserNameValidForUpdate(int userId, string username);
        List<UserDto> GetUserInGroupChat(int conversationId);
        UserDto GetUserDtoWithPhoto(int userId);

        bool CheckFriendUser(int userId, int otherUserId);

    }
}
