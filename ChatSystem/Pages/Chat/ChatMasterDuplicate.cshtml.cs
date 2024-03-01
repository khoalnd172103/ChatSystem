using AutoMapper;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Repository;
using Repository.DTOs;

namespace ChatSystem.Pages.Chat
{
    [Authorize]
    public class ChatMasterDuplicateModel : PageModel
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        private readonly IFriendRepository _friendRepository;
        private readonly IPhotoRepository _photoRepository;

        public ChatMasterDuplicateModel(IConversationRepository conversationRepository,
            IParticipantRepository participantRepository,
            IUserRepository userRepository,
            IMapper mapper, IMessageRepository messageRepository, 
            IFriendRepository friendRepository,
            IPhotoRepository photoRepository)
        {
            _conversationRepository = conversationRepository;
            _participantRepository = participantRepository;
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _messageRepository = messageRepository;
            _friendRepository = friendRepository;
            _photoRepository = photoRepository;
        }

        public List<UserDto> GroupChatParticipants { get; set; }
        public Conversation currentConversation { get; set; }
        public UserDto UserDto { get; set; }

        [BindProperty(SupportsGet = true)]
        public List<ConversationDto> ConversationDtoList { get; set; } = default!;

        [BindProperty]
        public ConversationDto conversationDto { get; set; }

        [BindProperty]
        public List<FriendDto> Friends { get; set; }

        [BindProperty]
        public List<string> SelectedFriends { get; set; }

        [BindProperty]
        public bool IsLastAdminLogined { get; set; } = false;

        [BindProperty]
        public bool IsLastMemberLogined { get; set; } = false;

        public IActionResult OnGet()
        {
            try
            {
                var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);

                if (idClaim != null)
                {
                    ConversationDtoList = new List<ConversationDto>();
                    int userId = int.Parse(idClaim.Value);

                    List<Conversation> conversationList = _conversationRepository.GetAllUserConversation(userId);
                    List<Conversation> conversationOrderList = conversationList.OrderByDescending(c => _messageRepository.GetLastestMessageFromConversation(c).DateSend.Ticks).ThenByDescending(c => c.CreateAt.Ticks).ToList();
                    //List<Conversation> conversationOrderList = conversationList.OrderByDescending(c => c.CreateAt.Ticks).ToList();

                    foreach (var conversation in conversationOrderList)
                    {
                        ConversationDto conversationDto = MapConversationToDto(conversation, userId);

                        ConversationDtoList.Add(conversationDto);
                    }

                    var conversationIdParam = Request.Query["id"];
                    int conversationId = int.Parse(conversationIdParam);

                    if (conversationId != null)
                    {

                        LoadConversation((int)conversationId);
                    }

                    IsLastAdminLogined = _participantRepository.IsLastAdminInConversation(conversationId, userId);
                    IsLastMemberLogined = _participantRepository.IsLastMemberInConversation(conversationId);

                    return Page();
                }
                return Page();

            }
            catch (Exception ex)
            {
                return Page();
            }

        }

        public IActionResult LoadConversation(int conversationId)
        {
            currentConversation = _conversationRepository.GetConversationById(conversationId);

            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            int userId = int.Parse(idClaim.Value);
            if (currentConversation == null)
            {
                return Page();
            }
            if (!_conversationRepository.IsUserInConversation(conversationId, userId))
            {
                return Page();
            }
            var user = _userRepository.GetUserWithPhoto(userId);
            if (user == null)
            {
                return NotFound();
            }


            UserDto = new UserDto
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




            GetConversationDetail(conversationId);

            conversationDto = MapConversationToDto(currentConversation, UserDto.UserId);


            return Page();
        }


        private ConversationDto MapConversationToDto(Conversation conversation, int userId)
        {
            ConversationDto conversationDto = _mapper.Map<Conversation, ConversationDto>(conversation);
            if (!conversationDto.isGroup)
            {
                User otherUser = _participantRepository.GetOtherParticipant(conversationDto.ConversationId, userId);
                conversationDto.OtherUserName = otherUser.KnownAs;
                conversationDto.OtherUserId = otherUser.UserId;
                conversationDto.Avatar = otherUser.photos.FirstOrDefault(p => p.isMain)?.PhotoUrl;
            }

            return conversationDto;
        }


        private void GetConversationDetail(int conversationId)
        {
            GroupChatParticipants = _userRepository.GetUserInGroupChat(conversationId);
            currentConversation = _conversationRepository.GetConversationById(conversationId);
        }

        public IActionResult PromoteUserToAdmin(int conversationId, int userId)
        {
            var participant = _participantRepository.GetParticipantByConversationIdAndUserId(conversationId, userId);

            if (participant != null)
            {
                participant.status = 1;
                participant.isAdmin = true;

                _participantRepository.Update(participant);
            }
            return Page();
        }

        public IActionResult KickUserFromGroup(int conversationId, int userId)
        {
            var participant = _participantRepository.GetParticipantByConversationIdAndUserId(conversationId, userId);

            if (participant != null)
            {
                participant.status = 0;

                _participantRepository.Update(participant);
            }
            return Page();
        }

        public IActionResult EditGroupName(int conversationId, string newGroupName) 
        {
            var conversation = _conversationRepository.GetConversationById(conversationId);

            if (conversation != null)
            {
                conversation.ConversationName = newGroupName;

                _conversationRepository.Update(conversation);
            }
            return Page();
        }



        public async Task<IActionResult> OnGetFriendListPartialAsync(int conversationId)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
            if (idClaim == null)
            {
                return RedirectToPage("/Account/Login");
            }

            int userId = int.Parse(idClaim.Value);

            var friends = await _friendRepository.GetFriendsNotInGroupAsync(userId, conversationId);

            Friends = friends.Select(friend => new FriendDto
            {
                UserId = friend.RecipientId,
                UserName = friend.RecipientUserName,
                Avatar = _photoRepository.GetUserPhotoIsMain(friend.RecipientId).PhotoUrl
            }).ToList();

            return Partial("_FriendListPartial", Friends);
        }

        public async Task<IActionResult> OnPostAddUserToGroup(int conversationId)
        {
            try
            {
                var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
                if (idClaim == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                int userId = int.Parse(idClaim.Value);
                _conversationRepository.AddUserToGroup(userId, conversationId, SelectedFriends);
                TempData["success"] = "Invite Successful";

                return RedirectToPage("/Chat/ChatMasterDuplicate", new { id = conversationId });
            }
            catch (Exception ex)
            {
                TempData["error"] = "Have an error " + ex.Message + " , try again";
                return LoadConversation(conversationId);
            }
        }

        public async Task<IActionResult> OnPostOutGroup(int conversationId)
        {
            try
            {
                var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId");
                if (idClaim == null)
                {
                    return RedirectToPage("/Account/Login");
                }

                int userId = int.Parse(idClaim.Value);

                IsLastMemberLogined = _participantRepository.IsLastMemberInConversation(conversationId);

                _participantRepository.OutConversation(conversationId, userId);
                if (IsLastMemberLogined)
                {
                    // Add method for delete conversation here
                }

                TempData["success"] = "Out Successful";

                return RedirectToPage("/Chat/ChatMasterDuplicate");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Have an error " + ex.Message + " , try again";
                return LoadConversation(conversationId);
            }
        }
    }
}
