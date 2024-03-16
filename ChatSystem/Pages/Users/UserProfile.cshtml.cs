using BusinessObject;
using ChatSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repository;
using Repository.DTOs;

namespace ChatSystem.Pages.Users
{
    public class UserProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> notificationContext;
        private readonly IFriendRepository friendRepository;
        private readonly IConversationRepository _conversationRepository;

        public UserProfileModel(IUserRepository userRepository, IFriendRepository friendRepository, IHubContext<NotificationHub> notificationContext, IConversationRepository conversationRepository)
        {
            _userRepository = userRepository;
            this.friendRepository = friendRepository;
            this.notificationContext = notificationContext;
            this._conversationRepository = conversationRepository;
        }

        public UserProfileDto UserProfile { get; set; }
        public bool IsLogined { get; set; } = false;
        public bool IsFriend { get; set; } = false;
        [BindProperty]
        public int UserId { get; set; }

        public int FriendStatus { get; set; } = 0;

        public IActionResult OnGet(int UserId)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
                IsFriend = friendRepository.CheckIsFriendAsync(int.Parse(idClaim.Value), UserId);
            }
            int loginUserId = int.Parse(idClaim.Value);

            var user = _userRepository.GetUserWithPhoto(UserId);
            if (user == null)
            {
                return NotFound();
            }

            if (user != null)
            {
                IsFriend = _userRepository.CheckFriendUser(loginUserId, UserId);

                if (!IsFriend)
                {
                    FriendStatus = friendRepository.CheckFriendForUser(loginUserId, UserId);
                }
                UserProfile = new UserProfileDto
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

            return Page();
        }

        public IActionResult OnPostStartConversationAsync()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }
            int loginUserId = int.Parse(idClaim.Value);

            var conversation = _conversationRepository.GetConversationBySenderIdAndReceiverId(loginUserId, UserId);

            if (conversation != null)
            {
                return RedirectToPage("/Chat/ChatMaster", new { id = conversation.ConversationId });
            }
            else
            {
                var sender = _userRepository.GetUser(loginUserId);
                var receiver = _userRepository.GetUser(UserId);

                var newConversation = new Conversation
                {
                    ConversationName = "Conversation of " + sender.UserName + " and " + receiver.UserName,
                    UserId = loginUserId,
                    CreateAt = DateTime.Now,
                    isGroup = false,
                    MessagesReceived = new List<Message>(),
                    Participants = new List<Participants>
        {
            new Participants { UserId = loginUserId, isAdmin = true, status = 1 },
            new Participants { UserId = UserId, isAdmin = false, status = 1 }
        }
                };

                _conversationRepository.Create(newConversation);

                return RedirectToPage("/Chat/ChatMaster", new { id = newConversation.ConversationId });
            }
        }

        public async Task<IActionResult> OnPostAddFriend()
        {
            

            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }
            HttpContext.Session.SetInt32("UserId", UserId);
            int currentUserId = int.Parse(idClaim.Value);
            var recipientUserName = _userRepository.GetUser(UserId).UserName;
            var senderUserName = _userRepository.GetUser(currentUserId).UserName;
            await friendRepository.SendFriendRequest(currentUserId, UserId, senderUserName, recipientUserName);
            await notificationContext.Clients.Group(UserId.ToString()).SendAsync("OnSendFriendRequest", "You receive a friend request from " + senderUserName);
            return RedirectToPage("/Users/UserProfile", new { userId = UserId });
        }

        public async Task<IActionResult> OnPostAcceptFriend()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }
            int loginUserId = int.Parse(idClaim.Value);
            await friendRepository.AcceptFriendRequest(UserId, loginUserId);
            await notificationContext.Clients.Group(UserId.ToString()).SendAsync("OnAcceptFriendRequest", "accept your friend request");
            return RedirectToPage("/Users/UserProfile", new { userId = UserId });
        }

        public async Task<IActionResult> OnPostDeclineFriend()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }
            int loginUserId = int.Parse(idClaim.Value);
            await friendRepository.DeclineFriendRequest(UserId, loginUserId);
            return RedirectToPage("/Users/UserProfile", new { userId = UserId });
        }

        public async Task<IActionResult> OnPostUnfriend()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }
            int loginUserId = int.Parse(idClaim.Value);
            await friendRepository.DeleteFriendAsync(UserId, loginUserId);
            return RedirectToPage("/Users/UserProfile", new { userId = UserId });
        }

    }
}
