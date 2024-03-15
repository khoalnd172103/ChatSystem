using BusinessObject;

namespace Repository
{
    public interface IConversationRepository : IBaseRepository<Conversation>
    {
        void CreateGroup(int creatorId, string groupName, List<string> memberIdList);
        void AddUserToGroup(int creatorId, int conversationId, List<string> memberIdList);

        List<Conversation> GetAllUserConversation(int userId);

        public Conversation GetConversationById(int conversationId);

        public Task<List<Conversation>> GetAllConversationById(int userID);
        bool IsUserInConversation(int conversationId, int userId);
        void UpdateConversation(Conversation conversation);
        void DeleteConversation(int conversationId);

        Conversation GetConversationBySenderIdAndReceiverId(int senderId, int receiverId);
    }
}
