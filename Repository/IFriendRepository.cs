using BusinessObject;
using Repository.DTOs;

namespace Repository
{
    public interface IFriendRepository : IBaseRepository<Friend>
    {
        public PaginatedList<FriendRequestDto> GetFriendRequest(int? pageIndex, int pageSize, string recipientUserName);
        IEnumerable<Friend> GetFriend();
        Task<List<Friend>> GetFriendsForUserAsync(int userId);
        Task<List<Friend>> SearchFriendsForUserAsync(int userId, string searchKey);
        Task<List<Friend>> SortByDateAsync(int userId, bool searchKey);
        Task UnfriendAsync(int userId, int friendId);
        Task SendFriendRequest(int senderId, int recipientId, string senderUsername, string recipientUsername);
        void UpdateFriendRequest(Friend friend);
        void DeclineFriendRequest(Friend friend);
        Task<List<Friend>> GetFriendsNotInGroupAsync(int userId, int convarsationId);
        PaginatedList<FriendListDto> GetFriendListForUser(string searchString, int? pageIndex, int pageSize, int userId);
        int CheckFriendForUser(int userId, int otherUserId);
        Task AcceptFriendRequest(int senderId, int recipientId);
        Task DeclineFriendRequest(int senderId, int recipientId);
        Task DeleteFriendAsync(int senderId, int recipientId);
        bool CheckIsFriendAsync(int userId, int otherUserId);
    }
}
