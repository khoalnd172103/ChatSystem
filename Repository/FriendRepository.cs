using BusinessObject;
using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly FriendDAO friendDAO;

        public FriendRepository()
        {
            friendDAO = new FriendDAO();
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
            throw new NotImplementedException();
        }

        public List<Friend> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Friend> GetFriendRequest(string recipientUserName)
        {
            return friendDAO.GetAll().Where(fr => fr.RecipientUserName == recipientUserName &&
                fr.status == false).ToList();
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
    }
}
