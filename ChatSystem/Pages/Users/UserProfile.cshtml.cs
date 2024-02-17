using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.ComponentModel.DataAnnotations;

namespace ChatSystem.Pages.Users
{
    public class UserProfile
    {
        public int UserId { get; set; }

        [Required(ErrorMessage ="UserName is required")]
        [UserNameValidation]
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string? KnownAs { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interest { get; set; }
        public string? City { get; set; }
    }

    public class UserProfileModel : PageModel
    {

        private readonly IUserRepository _userRepository;

        [BindProperty]
        public UserProfile UserProfile { get; set; }

        public UserProfileModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult OnGet()
        {
            int? userId = HttpContext.Session.GetInt32("UserId");
            if (userId.HasValue)
            {
                var user = _userRepository.GetUsers().SingleOrDefault(u => u.UserId.Equals(userId));
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
                }
                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = _userRepository.GetUsers().SingleOrDefault(u => u.UserId.Equals(UserProfile.UserId));

            if (user == null)
            {
                return NotFound();
            }

            user.UserName = UserProfile.UserName;
            user.DateOfBirth = UserProfile.DateOfBirth;
            user.KnownAs = UserProfile.KnownAs;
            user.Gender = UserProfile.Gender;
            user.Introduction = UserProfile.Introduction;
            user.Interest = UserProfile.Interest;
            user.City = UserProfile.City;

            _userRepository.UpdateUser(user);
            return RedirectToPage("/Users/UserProfile");
        }
    }
}
