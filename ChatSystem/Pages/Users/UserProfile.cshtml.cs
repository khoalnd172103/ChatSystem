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

        public UserProfile UserProfile { get; set; }
        public bool IsLogined { get; set; } = false;

        public IActionResult OnGet(int userId)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }

            var user = _userRepository.GetUserWithPhoto(userId);
            if (user == null)
            {
                return NotFound();
            }

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
                    City = user.City,
                    Avatar = user.photos.FirstOrDefault(p => p.isMain)?.PhotoUrl
                };
            }

            return Page();
        }
    }
}
