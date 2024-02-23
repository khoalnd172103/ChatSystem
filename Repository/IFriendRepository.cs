using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IFriendRepository : IBaseRepository<Friend>
    {
        public List<Friend> GetFriendRequest(string recipientUserName);
        IEnumerable<Friend> GetFriend();
        Task<List<Friend>> GetFriendsForUserAsync(int userId);
        Task<List<Friend>> SearchFriendsForUserAsync(int userId, string searchKey);
        Task<List<Friend>> SortByDateAsync(int userId, bool searchKey);
    }
}
