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

        public List<User> GroupChatParticipants { get; private set; }

        public ChatMasterModel(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }
        public void OnGet()
        {
            //hard code for test
            GroupChatParticipants = _userRepository.GetUserInGroupChat(1);
        }
    }
}
