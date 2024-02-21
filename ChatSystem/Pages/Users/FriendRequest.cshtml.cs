using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace ChatSystem.Pages.Users
{
    public class FriendRequestModel : PageModel
    {
        private readonly IFriendRepository friendRepository;
        private readonly IUserRepository userRepository;
        public int? userId { get; set; } = 0;
        public IEnumerable<Friend> Request { get; set; }
        public FriendRequestModel(IFriendRepository friendRepository, IUserRepository userRepository)
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
                int userID = (int)userId;
                string username = userRepository.GetUser(userID).UserName;
                Request = friendRepository.GetFriendRequest(username);
            }
        }
    }
}
