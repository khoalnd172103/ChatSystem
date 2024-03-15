using BusinessObject;
using ChatSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repository;

namespace ChatSystem.Pages.Users
{
    public class UserProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IHubContext<NotificationHub> notificationContext;
        private readonly IFriendRepository friendRepository;

        public UserProfileModel(IUserRepository userRepository, IFriendRepository friendRepository, IHubContext<NotificationHub> notificationContext)
        {
            _userRepository = userRepository;
            this.friendRepository = friendRepository;
            this.notificationContext = notificationContext;
        }

        public UserProfileDto UserProfile { get; set; }
        public bool IsLogined { get; set; } = false;
        public bool IsFriend { get; set; } = false;

        public int FriendStatus { get; set; } = 0;

        [BindProperty]
        public int UserId { get; set; }

        public IActionResult OnGet(int UserId)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
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

        public async Task<IActionResult> OnPostAddFriend()
        {
            await notificationContext.Clients.All.SendAsync("OnSendFriendRequest");

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
            //await friendRepository.DeclineFriendRequest(UserId, loginUserId);
            return RedirectToPage("/Users/UserProfile", new { userId = UserId });
        }
    }
}
