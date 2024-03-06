using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ChatSystem.Pages.Users
{
    public class FriendDetailModel : PageModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IUserRepository _userRepository;
        public UserProfileDto UserProfile { get; set; }


        public FriendDetailModel(IFriendRepository friendRepository, IUserRepository userRepository)
        {
            _friendRepository = friendRepository;
            _userRepository = userRepository;
            UserProfile = new UserProfileDto();
        }


        public IActionResult OnGet(int id)
        {
            var user = _userRepository.GetUser(id);
            if (user != null)
            {
                UserProfile = new UserProfileDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    DateOfBirth = user.DateOfBirth,
                    KnownAs = user.KnownAs,
                    Gender = user.Gender,
                    Introduction = user.Introduction,
                    Interest = user.Interest,
                    City = user.City
                };
                return Page();
            }
            else
            {
                return NotFound();
            }
        }


        public async Task<IActionResult> OnPost(int friendId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(claim => claim.Type == "UserId").Value);
            await _friendRepository.UnfriendAsync(userId, friendId);
            return RedirectToPage("/Users/FriendList");
        }
    }
}
