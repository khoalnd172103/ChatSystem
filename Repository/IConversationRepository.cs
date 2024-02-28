using BusinessObject;

namespace Repository
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        public void CreateGroup(int creatorId, string groupName, List<string> memberIdList);

        Task<List<Conversation>> GetAllUserConversation(int userId);

        public Conversation GetConversationById(int conversationId);
    }
}
