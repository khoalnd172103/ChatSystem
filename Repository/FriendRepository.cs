using BusinessObject;
using DataAccessLayer;
using Repository.DTOs;

namespace Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly FriendDAO friendDAO;
        private readonly IPhotoRepository photoRepository;

        public FriendRepository()
        {
            friendDAO = new FriendDAO();
            photoRepository = new PhotoRepository();
        }
        public void Create(Friend entity)
        {
            friendDAO.Create(entity);
        }

        public bool Delete(Friend entity)
        {
            throw new NotImplementedException();
        }

        public List<Friend> GetAll()
        {
            throw new NotImplementedException();
        }

        public Friend GetById(int entityId)
        {
            return friendDAO.GetAll().FirstOrDefault(u => u.RequestId == entityId);
        }

        public List<Friend> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public PaginatedList<FriendRequestDto> GetFriendRequest(int? pageIndex, int pageSize, string recipientUserName)
        {
            var friendRequest = friendDAO.GetAll().Where(fr => fr.RecipientUserName == recipientUserName &&
               fr.status == false).ToList();

            var friendRequestDto = friendRequest.Select(request => new FriendRequestDto
            {
                RequestId = request.RequestId,
                SenderId = request.SenderId,
                DateSend = request.DateSend,
                SenderName = request.SenderUserName,
                AvatarUrl = photoRepository.GetUserPhotoIsMain(request.SenderId).PhotoUrl
            }).ToList();

            return PaginatedList<FriendRequestDto>.CreateAsync(
                friendRequestDto.AsQueryable(), pageIndex ?? 1, pageSize);
        }

        public IEnumerable<Friend> GetFriend()
        => FriendDAO.Instance.GetAll();

        public bool Update(Friend entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Friend>> GetFriendsForUserAsync(int userId)
        => FriendDAO.Instance.GetFriendsForUser(userId);

        public Task<List<Friend>> SearchFriendsForUserAsync(int userId, string searchKey)
        => FriendDAO.Instance.SearchFriendsForUserAsync(userId, searchKey);

        public Task<List<Friend>> SortByDateAsync(int userId, bool searchKey)
        => FriendDAO.Instance.SortByDateAsync(userId, searchKey);

        public Task UnfriendAsync(int userId, int friendId)
        => FriendDAO.Instance.UnfriendAsync(userId, friendId);

        public Task SendFriendRequest(int senderId, int recipientId, string senderUsername, string recipientUsername)
            => FriendDAO.Instance.SendFriendRequest(senderId, recipientId, senderUsername, recipientUsername);

        public void UpdateFriendRequest(Friend friend)
        {
            friendDAO.Update(friend);
        }

        public void DeclineFriendRequest(Friend friend)
        {
            friendDAO.Remove(friend);
        }

        public Task<List<Friend>> GetFriendsNotInGroupAsync(int userId, int convarsationId)
        {
            return friendDAO.GetFriendsNotInGroup(userId, convarsationId);
        }
    }
}
