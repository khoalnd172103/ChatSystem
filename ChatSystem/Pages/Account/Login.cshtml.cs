using BusinessObject;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace ChatSystem.Pages.Account
{
    public class InputModel
    {
        [Required]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Display(Name = "Remember Me")]
        public bool RememberMe { get; set; }
    }
    public class LoginModel : PageModel
    {
        private readonly IUserRepository _userRepository;

        public LoginModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        [BindProperty]
        public InputModel Input { get; set; }


        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }



            User user = new User();
            try
            {
                user = _userRepository.Login(Input.Username, Input.Password);
                if (user != null)
                {

                    var claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.KnownAs),
                    new Claim("UserId", user.UserId.ToString()),
                    //new Claim("User", "true"),
                    //new Claim(ClaimTypes.Role, "User"),
                };

                    var identity = new ClaimsIdentity(claims, "CookieAuth");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);

                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = Input.RememberMe
                    };

                    await HttpContext.SignInAsync("CookieAuth", claimsPrincipal, authProperties);
                    HttpContext.Session.SetInt32("UserId", user.UserId);
                    return RedirectToPage("/Users/UserList");
                }
                else
                {
                    TempData["error"] = "Login fail";
                }
                return Page();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Login fail";
                return Page();
            }


        }
    }
}


