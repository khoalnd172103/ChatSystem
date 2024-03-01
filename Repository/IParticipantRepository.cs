using BusinessObject;

namespace Repository
{
    public interface IParticipantRepository : IBaseRepository<Participants>
    {
        User GetOtherParticipant(int conversationId, int userId);
        Participants GetParticipantByConversationIdAndUserId(int conversationId, int userId);
        void OutConversation(int conversationId, int userId);
        bool IsLastAdminInConversation(int conversationId, int userId);
        bool IsLastMemberInConversation(int conversationId);
    }
}
