using BusinessObject;

namespace Repository
{
    public interface IParticipantRepository : IBaseRepository<Participants>
    {
        User GetOtherParticipant(int conversationId, int userId);
        Participants GetParticipantByConversationIdAndUserId(int conversationId, int userId);
        public void UpdateParticipants(Participants participants);


    }
}
