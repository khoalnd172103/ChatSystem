using BusinessObject;

namespace Repository
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        public void CreateGroup(int creatorId, string groupName, List<string> memberIdList);
        public void AddUserToGroup(int creatorId, int conversationId, List<string> memberIdList);

        List<Conversation> GetAllUserConversation(int userId);

        public Conversation GetConversationById(int conversationId);

        public Task<List<Conversation>> GetAllConversationById(int userID);
        bool IsUserInConversation(int conversationId, int userId);
    }
}
