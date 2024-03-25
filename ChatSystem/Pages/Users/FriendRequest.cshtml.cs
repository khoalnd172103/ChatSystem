using BusinessObject;
using ChatSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Repository;
using Repository.DTOs;

namespace ChatSystem.Pages.Users
{
    public class FriendRequestModel : PageModel
    {
        private readonly IFriendRepository friendRepository;
        private readonly IUserRepository userRepository;
        private readonly IHubContext<NotificationHub> notificationContext;
        public int? userId { get; set; } = 0;
        public PaginatedList<FriendRequestDto> Request { get; set; }
        public FriendRequestModel(IFriendRepository friendRepository, IUserRepository userRepository, IPhotoRepository photoRepository, IHubContext<NotificationHub> notificationContext)
        {
            this.friendRepository = friendRepository;
            this.userRepository = userRepository;
            this.notificationContext = notificationContext;
        }

        public void OnGet()
        {
            var userIdClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            userId = int.Parse(userIdClaim.Value);

            LoadRequest();
        }

        private void LoadRequest()
        {
            if (userId != null)
            {
                const int pageSize = 5;
                int userID = (int)userId;
                string username = userRepository.GetUser(userID).UserName;
                Request = friendRepository.GetFriendRequest(1, pageSize, username);
            }
        }

        public async Task<IActionResult> OnPostAccept(int requestId)
        {
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var friendRequest = friendRepository.GetById(requestId);
            if (friendRequest == null)
            {
                return NotFound();
            }

            // Accept friend request logic here
            friendRequest.status = true;
            friendRepository.UpdateFriendRequest(friendRequest);
            await notificationContext.Clients.Group(friendRequest.SenderId.ToString()).SendAsync("OnAcceptFriendRequest", friendRequest.RecipientUserName + " accept your friend request");
            await notificationContext.Clients.Group(friendRequest.SenderId.ToString()).SendAsync("OnFriendRequestUpdate", 3);
            await notificationContext.Clients.Group(friendRequest.RecipientId.ToString()).SendAsync("OnFriendRequestUpdate", 3);
            return RedirectToPage("FriendRequest");
        }

        public async Task<IActionResult> OnPostDecline(int requestId)
        {
            if (userId == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }
            var friendRequest = friendRepository.GetById(requestId);
            if (friendRequest == null)
            {
                return NotFound();
            }
            friendRepository.DeclineFriendRequest(friendRequest);
            await notificationContext.Clients.Group(friendRequest.SenderId.ToString()).SendAsync("OnFriendRequestUpdate", 0);
            return RedirectToPage("FriendRequest");
        }
    }
}
