using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.ComponentModel.DataAnnotations;
using DataAccessLayer;
using BusinessObject;
using ChatSystem.Pages.Users.Validation;

namespace ChatSystem.Pages.Users
{
    public class EditProfile
    {
        public int UserId { get; set; }

        [Required(ErrorMessage = "UserName is required")]
        [BusinessObject.UserNameValidation]
        public string UserName { get; set; }
        [DataType(DataType.Date)]
        [BirthdayValidation(18)]
        public DateTime DateOfBirth { get; set; }
        [Required(ErrorMessage = "KnownAs is required")]
        public string KnownAs { get; set; }
        public string? Gender { get; set; }
        public string? Introduction { get; set; }
        public string? Interest { get; set; }
        public string? City { get; set; }
    }

    public class EditProfileModel : PageModel
    {

        private readonly IUserRepository _userRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly ICloudinaryService _cloudinary;

        [BindProperty]
        public EditProfile UserProfile { get; set; }

        [BindProperty]
        public string ? Avatar { get; set; }

        public EditProfileModel(IUserRepository userRepository, IPhotoRepository photoRepository, ICloudinaryService cloudinaryService)
        {
            _userRepository = userRepository;
            _photoRepository = photoRepository;
            _cloudinary = cloudinaryService;
            UserProfile = new EditProfile();
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
                    UserProfile.UserId = user.UserId;
                    UserProfile.UserName = user.UserName;
                    UserProfile.DateOfBirth = user.DateOfBirth;
                    UserProfile.KnownAs = user.KnownAs;
                    UserProfile.Gender = user.Gender;
                    UserProfile.Introduction = user.Introduction;
                    UserProfile.Interest = user.Interest;
                    UserProfile.City = user.City;

                    Avatar = _photoRepository.GetUserPhotoIsMain(userId).PhotoUrl;
                }

                return Page();
            }
            else
            {
                return RedirectToPage("/Account/Login");
            }
        }

        public IActionResult OnPostUpdateProfile()
        {
            if (!ModelState.IsValid)
            {
                return OnGet();
            }

            var user = _userRepository.GetUser(UserProfile.UserId);

            if (user == null)
            {
                return NotFound();
            }

            try
            {
                var isValidUserName = _userRepository.IsUserNameValidForUpdate(UserProfile.UserId, UserProfile.UserName);
                if (isValidUserName)
                {
                    ViewData["Message"] = "This username is already used";
                    return OnGet();
                }

                user.UserName = UserProfile.UserName;
                user.DateOfBirth = UserProfile.DateOfBirth;
                user.KnownAs = UserProfile.KnownAs;
                user.Gender = UserProfile.Gender;
                user.Introduction = UserProfile.Introduction;
                user.Interest = UserProfile.Interest;
                user.City = UserProfile.City;

                _userRepository.UpdateUser(user);

                TempData["success"] = "Update Successful";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Has error when update profile: " + ex.Message;
            }
            
            return RedirectToPage("/Users/EditProfile");
        }

        public async Task<IActionResult> OnPostUploadImage(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
            {
                ViewData["ChangeImageMessage"] = "Please select a file.";
                ModelState.Clear();
                return OnGet();
            }

            try
            {
                Photo photo = _photoRepository.GetUserPhotoIsMain(UserProfile.UserId);

                var result = await _cloudinary.AddPhotoAsync(imageFile);
                if (result.Error != null)
                {
                    ViewData["ChangeImageMessage"] = "Image upload failed. Please try again later.";
                    return Page();
                }

                photo.PhotoUrl = result.SecureUrl.AbsoluteUri;
                _photoRepository.UpdatePhoto(photo);

                TempData["success"] = "Change Successful";
            }
            catch (Exception ex)
            {
                TempData["error"] = "Has error when change avatar: " + ex.Message;
            }

            return RedirectToPage("/Users/EditProfile");
        }
    }
}
