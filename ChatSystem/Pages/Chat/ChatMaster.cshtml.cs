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
        private readonly IMessageRepository _messageRepository;
        private readonly IMapper _mapper;

        public ChatMasterModel(IConversationRepository conversationRepository, IParticipantRepository participantRepository, IMapper mapper, IMessageRepository messageRepository)
        {
            _conversationRepository = conversationRepository;
            _participantRepository = participantRepository;
            _mapper = mapper;
            _messageRepository = messageRepository;
        }

        [BindProperty(SupportsGet = true)]
        public List<ConversationDto> ConversationDtoList { get; set; } = default!;

        [BindProperty]
        public ConversationDto conversationDto { get; set; }

        public IActionResult OnGet()
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

                return Page();
            }
            return Page();

        }

        public async Task<IActionResult> OnPostLoadConversation()
        {


            return OnGet();
        }

    }
}
