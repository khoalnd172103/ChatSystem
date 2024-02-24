using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer;
using BusinessObject;

namespace ChatSystem.Pages.Users
{
    public class EditProfile
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [UserNameValidation]
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        public string? KnownAs { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interest { get; set; }
        public string? City { get; set; }
        public string? Avatar { get; set; }
    }

    public class EditProfileModel : PageModel
    {

        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ICloudinaryService _cloudinary;

        [BindProperty]
        public EditProfile UserProfile { get; set; }

        public EditProfileModel(IUserRepository userRepository, IPhotoRepository photoRepository, ICloudinaryService cloudinaryService)
        {
            _userRepository = userRepository;
            _photoRepository = photoRepository;
            _cloudinary = cloudinaryService;
        }

        public IActionResult OnGet()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);

            if (idClaim != null)
            {
                int userId = int.Parse(idClaim.Value);
                var user = _userRepository.GetUserWithPhoto(userId);
                if (user != null)
                {
                    UserProfile = new EditProfile
                    {
                        UserId = user.UserId,
                        UserName = user.UserName,
                        DateOfBirth = user.DateOfBirth,
                        KnownAs = user.KnownAs,
                        Gender = user.Gender,
                        Introduction = user.Introduction,
                        Interest = user.Interest,
                        City = user.City,
                    };
                    UserProfile.Avatar = _photoRepository.GetUserPhotoIsMain(userId).PhotoUrl;
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
            return RedirectToPage("/Users/EditProfile");
        }

        public async Task<IActionResult> OnPostUploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ModelState.AddModelError(string.Empty, "Please select a file.");
                return Page();
            }

            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            int userId = int.Parse(idClaim.Value);

            Photo photo = _photoRepository.GetUserPhotoIsMain(userId);

            var result = await _cloudinary.AddPhotoAsync(imageFile);
            if (result.Error != null)
            {
                ModelState.AddModelError(string.Empty, "Image upload failed. Please try again later.");
                return Page();
            }

            photo.PhotoUrl = result.SecureUrl.AbsoluteUri;
            _photoRepository.UpdatePhoto(photo);

            return RedirectToPage("/Users/EditProfile");
        }
    }
}
