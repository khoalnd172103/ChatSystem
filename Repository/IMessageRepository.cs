using BusinessObject;

namespace Repository
{
    public interface IMessageRepository : IBaseRepository<Message>
    {
        Message GetLastestMessageFromConversation(Conversation conversation);
    }
}
