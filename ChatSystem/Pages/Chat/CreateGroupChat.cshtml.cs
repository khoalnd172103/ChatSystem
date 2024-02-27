using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using Repository;
using Repository.DTOs;
using System.ComponentModel.DataAnnotations;

namespace ChatSystem.Pages.Chat
{
    public class CreateGroupChatModel : PageModel
    {
        private readonly IFriendRepository _friendRepository;
        private readonly IConversationRepository _conversationRepository;
        private readonly IPhotoRepository _photoRepository;

        public CreateGroupChatModel(IFriendRepository friendRepository, 
            IConversationRepository conversationRepository,
            IPhotoRepository photoRepository)
        {
            _friendRepository = friendRepository;
            _conversationRepository = conversationRepository;
            _photoRepository = photoRepository;
        }

        [BindProperty]
        public List<FriendDto> Friends { get; set; }

        [BindProperty]
        [Required(ErrorMessage ="Group name is required")]
        public string GroupName { get; set; }

        [BindProperty]
        public List<string> SelectedFriends { get; set; }

        public List<string> SelectedFriendIds { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
            if (idClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(idClaim.Value);

            var friends = await _friendRepository.GetFriendsForUserAsync(userId);

            Friends = friends.Select(friend => new FriendDto
            {
                UserId = friend.RecipientId,
                UserName = friend.RecipientUserName,
                Avatar = _photoRepository.GetUserPhotoIsMain(friend.RecipientId).PhotoUrl
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                TempData["error"] = "Group name is required";
                return await OnGetAsync();
            }

            try
            {
                SelectedFriendIds = JsonConvert.DeserializeObject<List<string>>(SelectedFriends[SelectedFriends.Count - 1]);

                var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
                if (idClaim == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                int userId = int.Parse(idClaim.Value);


                _conversationRepository.CreateGroup(userId, GroupName, SelectedFriendIds);
                TempData["success"] = "Create Successful";

                return RedirectToPage("/Chat/ChatMaster");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Have an error " + ex.Message + " , try again";
                return await OnGetAsync();
            }
        }
    }
}
