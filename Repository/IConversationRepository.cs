using BusinessObject;

namespace Repository
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        public void CreateGroup(int creatorId, string groupName, List<string> memberIdList);

        public Conversation GetConversationById(int conversationId);
    }
}
