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
    public class ChatMasterModel : PageModel
    {
        private readonly IConversationRepository _conversationRepository;
        private readonly IParticipantRepository _participantRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;

        public ChatMasterModel(IConversationRepository conversationRepository, 
            IParticipantRepository participantRepository,
            IUserRepository userRepository, 
            IMapper mapper)
        {
            _conversationRepository = conversationRepository;
            _participantRepository = participantRepository;
            _conversationRepository = conversationRepository;
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public List<User> GroupChatParticipants { get; set; }
        public Conversation Conversation { get; set; }

        public void OnGet()
        {
            //hard code for test
            int conversationId = 1;
            GroupChatParticipants = _userRepository.GetUserInGroupChat(conversationId);
            Conversation = _conversationRepository.GetConversationById(conversationId);
            
        }

        [BindProperty(SupportsGet = true)]
        public List<ConversationDto> ConversationDtoList { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var idClaim = User.Claims.FirstOrDefault(claims => claims.Type == "UserId", null);

            if (idClaim != null)
            {
                ConversationDtoList = new List<ConversationDto>();
                int userId = int.Parse(idClaim.Value);
                List<Conversation> conversationList = await _conversationRepository.GetAllUserConversation(userId);

                foreach (var conversation in conversationList)
                {
                    ConversationDto conversationDto = _mapper.Map<Conversation, ConversationDto>(conversation);
                    if (!conversationDto.isGroup)
                    {
                        User otherUser = _participantRepository.GetOtherParticipant(conversationDto.ConversationId, userId);
                        conversationDto.OtherUserName = otherUser.KnownAs;
                        conversationDto.OtherUserId = otherUser.UserId;
                        conversationDto.Avatar = otherUser.photos.FirstOrDefault(p => p.isMain)?.PhotoUrl;
                    }

                    ConversationDtoList.Add(conversationDto);
                }

            }
        }


    }
}
