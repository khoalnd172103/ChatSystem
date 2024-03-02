using AutoMapper;
using BusinessObject;
using Repository.DTOs;

namespace ChatSystem.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<Conversation, ConversationDto>();
            CreateMap<User, UserDto>();
            CreateMap<Message, MessageDto>();
        }
    }
}
