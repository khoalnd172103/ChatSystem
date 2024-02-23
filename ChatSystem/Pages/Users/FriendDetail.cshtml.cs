using BusinessObject;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using Repository;

namespace ChatSystem.Pages.Users
{
    public class FriendDetailModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public FriendDetailModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserProfile UserProfile { get; set; }

        public IActionResult OnGet(int id)
        {
            var user = _userRepository.GetUsers().SingleOrDefault(u => u.UserId == id);
            if (user != null)
            {
                UserProfile = new UserProfile
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
    }
}
