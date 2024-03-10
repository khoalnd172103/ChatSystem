using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;

namespace ChatSystem.Pages.Users
{
    public class UserProfileModel : PageModel
    {
        private readonly IUserRepository _userRepository;
        private readonly IConversationRepository _conversationRepository;

        public UserProfileModel(IUserRepository userRepository, IConversationRepository conversationRepository)
        {
            _userRepository = userRepository;
            _conversationRepository = conversationRepository;
        }

        public UserProfileDto UserProfile { get; set; }
        public bool IsLogined { get; set; } = false;
        public bool IsFriend { get; set; } = false;
        [BindProperty]
        public int UserId { get; set; }

        public IActionResult OnGet(int UserId)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
                IsFriend = _userRepository.CheckFriendUser(int.Parse(idClaim.Value), UserId);
            }
            //int loginUserId = int.Parse(idClaim.Value);

            var user = _userRepository.GetUserWithPhoto(UserId);
            if (user == null)
            {
                return NotFound();
            }

            if (user != null)
            {
                UserProfile = new UserProfileDto
                {
                    UserId = user.UserId,
                    UserName = user.UserName,
                    DateOfBirth = user.DateOfBirth,
                    KnownAs = user.KnownAs,
                    Gender = user.Gender,
                    Introduction = user.Introduction,
                    Interest = user.Interest,
                    City = user.City,
                    Avatar = user.photos.FirstOrDefault(p => p.isMain)?.PhotoUrl
                };
            }

            return Page();
        }

        public IActionResult OnPostStartConversationAsync()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            if (idClaim != null)
            {
                IsLogined = true;
            }
            int loginUserId = int.Parse(idClaim.Value);

            var conversation = _conversationRepository.GetConversationBySenderIdAndReceiverId(loginUserId, UserId);

            if (conversation != null)
            {
                return RedirectToPage("/Chat/ChatMaster", new { id = conversation.ConversationId });
            }
            else
            {
                var sender = _userRepository.GetUser(loginUserId);
                var receiver = _userRepository.GetUser(UserId);

                var newConversation = new Conversation
                {
                    ConversationName = "Conversation of " + sender.UserName + " and " + receiver.UserName,
                    UserId = loginUserId,
                    CreateAt = DateTime.Now,
                    isGroup = false,
                    MessagesReceived = new List<Message>(),
                    Participants = new List<Participants>
                {
                    new Participants { UserId = loginUserId, isAdmin = true, status = 1 },
                    new Participants { UserId = UserId, isAdmin = false, status = 1 }
                }
                };

                _conversationRepository.Create(newConversation);

                return RedirectToPage("/Chat/ChatMaster", new { id = newConversation.ConversationId });
            }
        }
    }
}
