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
    }
}
