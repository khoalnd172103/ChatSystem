using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace ChatSystem.Pages.Users
{
    public class UserProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public UserProfileModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public UserProfileDto UserProfile { get; set; }
        public bool IsLogined { get; set; } = false;
        public bool IsFriend { get; set; } = false;

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
    }
}
