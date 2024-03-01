using BusinessObject;
using Repository.DTOs;

namespace Repository
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Message GetLastestMessageFromConversation(Conversation conversation);
        List<MessageDto> GetMessagesFromConversation(Conversation conversation, List<UserDto> userDtos);
    }
}
