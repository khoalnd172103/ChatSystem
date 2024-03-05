using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.DTOs;

namespace ChatSystem.Pages.Users
{
    public class UserListModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IFriendRepository _friendRepository;
        public PaginatedList<UserDto> Users { get; set; }
        public string CurrentFilter { get; set; }
        public bool IsLogined { get; set; } = false;

        public UserListModel(IUserRepository userRepository, IFriendRepository friendRepository)
        {
            _userRepository = userRepository;
            _friendRepository = friendRepository;
        }

        public IActionResult OnGet(string searchString, int? pageIndex)
        {
            const int pageSize = 4;
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }

            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = CurrentFilter;
            }

            CurrentFilter = searchString;

            if (idClaim != null)
            {
                int userId = int.Parse(idClaim.Value);
                Users = _userRepository.GetUsers(searchString, pageIndex ?? 1, pageSize, userId);
            }
            else
            {
                Users = _userRepository.GetUsers(searchString, pageIndex ?? 1, pageSize, 0);
            }

            return Page();
        }

        public async Task<IActionResult> OnPost(int userId)
        {

            int? currentUserId = HttpContext.Session.GetInt32("UserId");
            if (currentUserId != null)
            {
                var senderId = (int)currentUserId;
                var recipientUserName = _userRepository.GetUser(userId).UserName;
                var senderUserName = _userRepository.GetUser(senderId).UserName;
                await _friendRepository.SendFriendRequest(senderId, userId, senderUserName, recipientUserName);
            }
            return RedirectToPage("/Users/UserList");
        }
    }
}
