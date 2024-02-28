using BusinessObject;
using DataAccessLayer;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace ChatSystem.Pages.Chat
{
    public class ChatMasterModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IConversationRepository _conversationRepository;

        public List<User> GroupChatParticipants { get; set; }
        public Conversation Conversation { get; set; }

        public ChatMasterModel(IUserRepository userRepository, IConversationRepository conversationRepository)
        {
            _userRepository = userRepository;
            _conversationRepository = conversationRepository;
        }
        public void OnGet()
        {
            //hard code for test
            int conversationId = 1;
            GroupChatParticipants = _userRepository.GetUserInGroupChat(conversationId);
            Conversation = _conversationRepository.GetConversationById(conversationId);
            
        }
    }
}
