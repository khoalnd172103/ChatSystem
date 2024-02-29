using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using System.Text.RegularExpressions;

namespace ChatSystem.Pages.Users
{
    public class UserGroupModel : PageModel
    {

        private readonly IConversationRepository _conversationRepository;
        private readonly IUserRepository _userRepository;

        public UserGroupModel(IConversationRepository conversationRepository, IUserRepository userRepository)
        {
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
        }
        public List<User> GroupChatParticipants { get; set; }
        public List<Conversation> Conversation { get; set; }
        public User UserObj { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
            if (idClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(idClaim.Value);

            UserObj = _userRepository.GetUser(userId);

            Conversation = await _conversationRepository.GetAllConversationById(userId);

            var groupData = new List<(string, List<string>)>();

            foreach (var conversation in Conversation)
            {
                if (conversation.isGroup)
                {
                    var participants = _userRepository.GetUserInGroupChat(conversation.ConversationId);

                    var participantNames = participants.Select(p => p.KnownAs).ToList();

                    groupData.Add((GroupName: conversation.ConversationName, Members: participantNames));
                }
            }

            ViewData["GroupData"] = groupData;

            return Page();
        }



    }
}
