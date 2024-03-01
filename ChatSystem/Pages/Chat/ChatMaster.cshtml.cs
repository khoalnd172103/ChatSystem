using AutoMapper;
using BusinessObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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

        public ChatMasterModel(IConversationRepository conversationRepository,
            IParticipantRepository participantRepository,
            IUserRepository userRepository,
            IMapper mapper, IMessageRepository messageRepository)
        {
            _conversationRepository = conversationRepository;
            _participantRepository = participantRepository;
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
            _messageRepository = messageRepository;
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
        public ChatContentModelDto ChatContentModel { get; set; }

        public List<MessageDto> MessageDtoList { get; set; } = default!;

        [BindProperty]
        public string MessageContent { get; set; }

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

        public IActionResult OnPostSendMessage()
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
            GetConversationDetail(conversationDto.ConversationId);

            UserDto = _userRepository.GetUserDtoWithPhoto(userId);
            GroupChatParticipants = _userRepository.GetUserInGroupChat(conversationDto.ConversationId);
            MessageDtoList = _messageRepository.GetMessagesFromConversation(currentConversation, GroupChatParticipants);
            ChatContentModel = new ChatContentModelDto
            {
                MessageDtoList = MessageDtoList,
                UserDto = UserDto
            };


            return Partial("_ChatContent", ChatContentModel);
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

    }
}
