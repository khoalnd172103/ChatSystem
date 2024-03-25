using BusinessObject;
using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;

namespace DataAccessLayer
{
    public class FriendDAO : BaseDAO<Friend>
    {
        public FriendDAO() { }

        private static FriendDAO instance = null;
        private static readonly object instacelock = new object();

        public static FriendDAO Instance
        {
            get
            {
                lock (instacelock)
                {
                    if (instance == null)
                    {
                        instance = new FriendDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Friend> CheckRequestFriend(int loginedId, int userId)
        {
            using (var context = new DataContext())
            {
                var friendRequest = context.Friend
                    .Where(f => (f.SenderId == loginedId && f.RecipientId == userId && f.status == true) || (f.SenderId == userId && f.RecipientId == loginedId && f.status == true))
                    .ToList();

                return friendRequest;
            }
        }

        public List<Friend> CheckFriendForUser(int userId)
        {
            using (var context = new DataContext())
            {
                var friends = context.Friend
                    .Where(f => (f.SenderId == userId) || (f.RecipientId == userId))
                    .ToList();
                return friends;
            }
        }

        public IEnumerable<Friend> GetFriendsListForUser(int userId)
        {
            var db = new DataContext();
            return db.Friend
                .Include(f => f.SenderUser).ThenInclude(u => u.photos)
                .Include(f => f.RecipientUser).ThenInclude(u => u.photos)
                .Where(f => (f.SenderId == userId && f.status == true) || (f.RecipientId == userId && f.status == true)).ToList();
        }

        public bool CheckIsFriend(int userId, int otherUserId)
        {
            using (var context = new DataContext())
            {
                var isFriend = context.Friend
                    .Where(f => (f.SenderId == userId && f.RecipientId == otherUserId && f.status == true) ||
                                (f.RecipientId == userId && f.SenderId == otherUserId && f.status == true))
                    .Any();

                return isFriend;
            }
        }

        public async Task<List<Friend>> SearchFriendsForUserAsync(int userId, string searchKey)
        {
            using (var context = new DataContext())
            {
                var friendsSent = await context.Friend
                    .Include(f => f.RecipientUser)
                    .Where(f => f.SenderId == userId && f.RecipientUser.UserName.Contains(searchKey))
                    .ToListAsync();

                var friendsReceived = await context.Friend
                    .Include(f => f.SenderUser)
                    .Where(f => f.RecipientId == userId && f.SenderUser.UserName.Contains(searchKey))
                    .ToListAsync();

                var allFriends = friendsSent.Concat(friendsReceived).ToList();

                return allFriends;
            }
        }

        public async Task<List<Friend>> SortByDateAsync(int userId, bool isAscending)
        {
            using (var context = new DataContext())
            {
                var friendsSent = await context.Friend
                    .Include(f => f.RecipientUser)
                    .Where(f => f.SenderId == userId)
                    .ToListAsync();

                var friendsReceived = await context.Friend
                    .Include(f => f.SenderUser)
                    .Where(f => f.RecipientId == userId)
                    .ToListAsync();

                var allFriends = friendsSent.Concat(friendsReceived);

                if (isAscending)
                {
                    allFriends = allFriends.OrderBy(f => f.DateSend);
                }
                else
                {
                    allFriends = allFriends.OrderByDescending(f => f.DateSend);
                }

                return allFriends.ToList();
            }
        }

        public async Task UnfriendAsync(int userId, int friendId)
        {
            using (var context = new DataContext())
            {
                var friend = await context.Friend.FirstOrDefaultAsync(f =>
               (f.SenderId == userId && f.RecipientId == friendId) ||
               (f.SenderId == friendId && f.RecipientId == userId));

                if (friend != null)
                {
                    friend.status = false;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task AcceptFriendRequestAsync(int senderId, int recipientId)
        {
            using (var context = new DataContext())
            {
                var request = await context.Friend.FirstOrDefaultAsync(f => (f.SenderId == senderId && f.RecipientId == recipientId && f.status == false));

                if (request != null)
                {
                    request.status = true;
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeclineFriendRequestAsysc(int senderId, int recipientId)
        {
            using (var context = new DataContext())
            {
                var request = await context.Friend.FirstOrDefaultAsync(f => (f.SenderId == senderId && f.RecipientId == recipientId && f.status == false));

                if(request != null)
                {
                    context.Friend.Remove(request);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task DeleteFriendAsync(int senderId, int recipientId)
        {
            using (var context = new DataContext())
            {
                var request = await context.Friend.FirstOrDefaultAsync(f => (f.SenderId == senderId && f.RecipientId == recipientId && f.status == true) || (f.SenderId == recipientId && f.RecipientId == senderId && f.status == true));

                if (request != null)
                {
                    context.Friend.Remove(request);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task SendFriendRequest(int senderId, int recipientId, string senderUsername, string recipientUsername)
        {
            using (var context = new DataContext())
            {
                var existingRequest = await context.Friend.FirstOrDefaultAsync(f =>
            (f.SenderId == senderId && f.RecipientId == recipientId) ||
            (f.SenderId == recipientId && f.RecipientId == senderId));

                // Check if a friend request already exists between these two users
                if (existingRequest != null)
                {
                    return;
                }

                // Create a new Friend object to represent the friend request
                var friendRequest = new Friend
                {
                    SenderId = senderId,
                    SenderUserName = senderUsername,
                    RecipientId = recipientId,
                    RecipientUserName = recipientUsername,
                    DateSend = DateTime.Now,
                    status = false
                };

                await context.Friend.AddAsync(friendRequest);
                await context.SaveChangesAsync();
            }
        }

        public async Task<List<Friend>> GetFriendsNotInGroup(int userId, int conversationId)
        {
            using (var context = new DataContext())
            {
                var participantUserIds = await context.Participants
                    .Where(p => p.ConversationId == conversationId)
                    .Select(p => p.UserId)
                    .ToListAsync();

                var friendsNotInGroup = await context.Friend
                    .Where(f =>
                        (f.SenderId == userId || f.RecipientId == userId) &&
                        (!participantUserIds.Contains(f.RecipientId) || !participantUserIds.Contains(f.SenderId)) && 
                        f.status == true)
                    .ToListAsync();

                var participantsWithStatusZero = await context.Participants
                    .Where(p => p.ConversationId == conversationId && p.status == 0)
                    .Select(p => p.UserId)
                    .ToListAsync();

                var friendsOutOfGroup = await context.Friend
                    .Where(f =>
                        (f.SenderId == userId && participantsWithStatusZero.Contains(f.RecipientId)) || 
                        (f.RecipientId == userId && participantsWithStatusZero.Contains(f.SenderId)) && 
                        f.status == true)
                    .ToListAsync();

                friendsNotInGroup.AddRange(friendsOutOfGroup);

                return friendsNotInGroup;
            }
        }
    }
}
