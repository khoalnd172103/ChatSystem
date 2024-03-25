using BusinessObject;
using ChatSystem.Hubs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
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
        private readonly IHubContext<MessageNotificationHub> _messageNotificationHubContext;

        public CreateGroupChatModel(IFriendRepository friendRepository, 
            IConversationRepository conversationRepository,
            IPhotoRepository photoRepository,
            IHubContext<MessageNotificationHub> messageNotificationHubContext)
        {
            _friendRepository = friendRepository;
            _conversationRepository = conversationRepository;
            _photoRepository = photoRepository;
            _messageNotificationHubContext = messageNotificationHubContext;
        }

        [BindProperty]
        public List<FriendDto> Friends { get; set; }
        [BindProperty]
        [Required(ErrorMessage ="Group name is required")]
        public string GroupName { get; set; }
        [BindProperty]
        public List<string> SelectedFriends { get; set; }
        public List<string> SelectedFriendIds { get; set; }
        public Conversation Conversation { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
            if (idClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(idClaim.Value);

            Friends = new List<FriendDto>();
            var friends = await _friendRepository.GetFriendsForUserAsync(userId);

            var friendsCurrentUserIsRecipient = friends.Where(f => f.RecipientId == userId).ToList();
            Friends.AddRange(friendsCurrentUserIsRecipient.Select(friend => new FriendDto
            {
                UserId = friend.SenderId,
                UserName = friend.SenderUserName,
                Avatar = _photoRepository.GetUserPhotoIsMain(friend.SenderId).PhotoUrl
            }));

            var friendsCurrentUserIsSender = friends.Where(f => f.SenderId == userId).ToList();
            Friends.AddRange(friendsCurrentUserIsSender.Select(friend => new FriendDto
            {
                UserId = friend.RecipientId,
                UserName = friend.RecipientUserName,
                Avatar = _photoRepository.GetUserPhotoIsMain(friend.RecipientId).PhotoUrl
            }));

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

                if (SelectedFriendIds != null)
                {
                    Conversation = _conversationRepository.CreateGroup(userId, GroupName, SelectedFriendIds);
                }
                else
                {
                    TempData["error"] = "Have an error, try again";
                    return await OnGetAsync();
                }
                
                if (Conversation != null)
                {
                    TempData["success"] = "Create Successful";

                    foreach (var par in SelectedFriendIds)
                    {
                        await _messageNotificationHubContext.Clients.Group(par).SendAsync("OnNewMessageReceived", "You are invited into " 
                            + Conversation.ConversationName, Conversation.ConversationId);
                    }

                    return RedirectToPage("/Chat/ChatMaster");
                }

                TempData["error"] = "Have an error, try again";
                return await OnGetAsync();
            }
            catch (Exception ex)
            {
                TempData["error"] = "Have an error " + ex.Message + " , try again";
                return await OnGetAsync();
            }
        }
    }
}
