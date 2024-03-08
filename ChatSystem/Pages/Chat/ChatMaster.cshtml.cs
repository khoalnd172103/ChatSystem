using AutoMapper;
using BusinessObject;
using ChatSystem.Hubs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.IdentityModel.Tokens;
using Repository;
using Repository.DTOs;

namespace ChatSystem.Pages.Chat
{
    [Authorize]
    public class ChatMasterModel : PageModel
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        private readonly IFriendRepository _friendRepository;
        private readonly IPhotoRepository _photoRepository;
        private readonly IHubContext<MessageHub> _messageHubContext;

        public ChatMasterModel(IConversationRepository conversationRepository,
            IParticipantRepository participantRepository,
            IUserRepository userRepository,
            IMapper mapper, IMessageRepository messageRepository,
            IFriendRepository friendRepository,
            IPhotoRepository photoRepository, IHubContext<MessageHub> messageHubContext)
        {
            _conversationRepository = conversationRepository;
            _participantRepository = participantRepository;
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _messageRepository = messageRepository;
            _friendRepository = friendRepository;
            _photoRepository = photoRepository;
            _messageHubContext = messageHubContext;
        }

        public List<UserDto> GroupChatParticipants
        { get; set; }
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
        public ChatContentModelDto ChatContentModel { get; set; }

        public List<MessageDto> MessageDtoList { get; set; } = default!;

        [BindProperty]
        public string MessageContent { get; set; }

        [BindProperty]
        public bool IsLastAdminLogined { get; set; } = false;

        [BindProperty]
        public bool IsLastMemberLogined { get; set; } = false;

        //private Dictionary<int, UserDto> UserDtoDictionary { get; set; }

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

        public IActionResult OnGetAgain(int conversationId)
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

                    LoadConversation((int)conversationId);

                    return Page();
                }
                return Page();

            }
            catch (Exception ex)
            {
                return Page();
            }

        }

        public IActionResult OnGetLoadMessage(int conversationId)
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            int userId = int.Parse(idClaim.Value);

            GetConversationDetail(conversationId);

            UserDto = _userRepository.GetUserDtoWithPhoto(userId);
            GroupChatParticipants = _userRepository.GetUserInGroupChat(conversationId);
            MessageDtoList = _messageRepository.GetMessagesFromConversation(currentConversation, GroupChatParticipants);
            ChatContentModel = new ChatContentModelDto
            {
                MessageDtoList = MessageDtoList,
                UserDto = UserDto
            };

            return Partial("_ChatContent", ChatContentModel);
        }

        public async Task OnPostSendMessage()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);
            int userId = int.Parse(idClaim.Value);
            if (!MessageContent.IsNullOrEmpty())
            {
                Message message = new Message();

                message.ConversationId = conversationDto.ConversationId;
                message.SenderId = userId;
                message.SenderDelete = false;
                message.Content = MessageContent;

                _messageRepository.Create(message);
            }

            await _messageHubContext.Clients.All.SendAsync("OnSendMessage", conversationDto.ConversationId);

            GetConversationDetail(conversationDto.ConversationId);

            UserDto = _userRepository.GetUserDtoWithPhoto(userId);
            GroupChatParticipants = _userRepository.GetUserInGroupChat(conversationDto.ConversationId);
            MessageDtoList = _messageRepository.GetMessagesFromConversation(currentConversation, GroupChatParticipants);
            ChatContentModel = new ChatContentModelDto
            {
                MessageDtoList = MessageDtoList,
                UserDto = UserDto
            };
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
            UserDto = _userRepository.GetUserDtoWithPhoto(userId);
            if (UserDto == null)
            {
                return NotFound();
            }

            GetConversationDetail(conversationId);

            conversationDto = MapConversationToDto(currentConversation, UserDto.UserId);

            MessageDtoList = _messageRepository.GetMessagesFromConversation(currentConversation, GroupChatParticipants);
            ChatContentModel = new ChatContentModelDto
            {
                MessageDtoList = MessageDtoList,
                UserDto = UserDto
            };

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

        public IActionResult OnPostPromoteUserToAdmin(int conversationId, int userId)
        {
            try
            {
                var participant = _participantRepository.GetParticipantByConversationIdAndUserId(conversationId, userId);

                if (participant != null)
                {
                    participant.status = 1;
                    participant.isAdmin = true;

                    _participantRepository.UpdateParticipants(participant);
                }
                TempData["success"] = "Update Successful";
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
            catch (Exception ex)
            {
                TempData["error"] = "Have an error " + ex.Message + " , try again";
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
        }

        public IActionResult OnPostKickUserFromGroup(int conversationId, int userId)
        {
            try
            {
                var participant = _participantRepository.GetParticipantByConversationIdAndUserId(conversationId, userId);

                if (participant != null)
                {
                    participant.status = 0;

                    _participantRepository.UpdateParticipants(participant);
                }
                TempData["success"] = "User kicked from the group successfully.";
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred while kicking the user: " + ex.Message;
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
        }

        public IActionResult OnPostEditGroupName(int conversationId, string newGroupName)
        {
            try
            {
                var conversation = _conversationRepository.GetConversationById(conversationId);

                if (conversation != null)
                {
                    conversation.ConversationName = newGroupName;

                    _conversationRepository.UpdateConversation(conversation);
                }
                TempData["success"] = "Group Name Updated Successfully";
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred: " + ex.Message + ". Please try again.";
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
        }

        public IActionResult OnPostDeleteGroup(int conversationId)
        {
            try
            {
                _conversationRepository.DeleteConversation(conversationId);
                TempData["success"] = "Delete Group Successfully";
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
            catch (Exception ex)
            {
                TempData["error"] = "An error occurred: " + ex.Message + ". Please try again.";
                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
            }
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

                return RedirectToPage("/Chat/ChatMaster", new { id = conversationId });
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
                    _conversationRepository.DeleteConversation(conversationId);
                }

                TempData["success"] = "Out Successful";

                return RedirectToPage("/Chat/ChatMaster");
            }
            catch (Exception ex)
            {
                TempData["error"] = "Have an error " + ex.Message + " , try again";
                return LoadConversation(conversationId);
            }
        }
    }
}
