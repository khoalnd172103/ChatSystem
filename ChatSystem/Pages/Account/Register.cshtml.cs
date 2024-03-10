using BusinessObject;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.Security.Claims;

namespace ChatSystem.Pages.Account
{

    public class RegisterModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public RegisterModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public IActionResult OnGet()
        {
            return Page();
        }


        public async Task<IActionResult> OnPostAsync()
        {
            var now = DateTime.Now.ToString("yyyy/MM/dd");
            if (!ModelState.IsValid || _userRepository == null || User == null)
            {
                return Page();
            }

            if (_userRepository.IsUsernameDuplicate(User.UserName))
            {
                ModelState.AddModelError("User.UserName", "This Username already existed!");
                return Page();
            }
            if (DateTime.Now.AddYears(-118).CompareTo(User.DateOfBirth) >= 0 || DateTime.Now.AddYears(-18).CompareTo(User.DateOfBirth) <= 0)
            {
                ModelState.AddModelError("User.DateOfBirth", "Must between 18 and 100 year old");
                return Page();
            }
            try
            {
                _userRepository.CreateUser(User);
                TempData["success"] = "Register successful";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Register fail";

            }
            var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, User.KnownAs),
                    new Claim("UserId", User.UserId.ToString()),
                    //new Claim("User", "true"),
                    //new Claim(ClaimTypes.Role, "User"),
                };

            var identity = new ClaimsIdentity(claims, "CookieAuth");
            ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync("CookieAuth", claimsPrincipal);
            //HttpContext.Session.SetInt32("UserId", user.UserId);
            TempData["success"] = "Register successful";

            return RedirectToPage("/Users/UserList");

        }
    }


}
