using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.DTOs;

namespace ChatSystem.Pages.Users
{
    public class FriendRequestModel : PageModel
    {
        private readonly IFriendRepository friendRepository;
        private readonly IUserRepository userRepository;
        public int? userId { get; set; } = 0;
        public PaginatedList<FriendRequestDto> Request { get; set; }
        public FriendRequestModel(IFriendRepository friendRepository, IUserRepository userRepository, IPhotoRepository photoRepository)
        {
            this.friendRepository = friendRepository;
            this.userRepository = userRepository;
        }

        public void OnGet()
        {
            userId = HttpContext.Session.GetInt32("UserId");
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
            return RedirectToPage("FriendRequest");
        }
    }
}
