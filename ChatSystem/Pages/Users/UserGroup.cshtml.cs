using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        public Conversation Conversation1 { get; set; }
        public List<UserDto> GroupChatParticipants { get; set; }
        public List<Conversation> Conversations { get; set; }
        public User UserObj { get; set; }

        public async Task<IActionResult> OnGetAsync(int conversationId)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
            if (idClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(idClaim.Value);

            UserObj = _userRepository.GetUser(userId);

            GroupChatParticipants = _userRepository.GetUserInGroupChat(conversationId);

            Conversation1 = _conversationRepository.GetConversationById(conversationId);

            return Page();
        }
    }
}
