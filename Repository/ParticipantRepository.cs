using BusinessObject;
using DataAccessLayer;

namespace Repository
{
    public class ParticipantRepository : IParticipantRepository
    {

        public User GetOtherParticipant(int conversationId, int userId)
        {
            User user = ParticipantDAO.Instance.GetOtherParticipantWhenNotGroupConversation(conversationId, userId);

            return user;
        }

        public void Create(Participants entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Participants entity)
        {
            throw new NotImplementedException();
        }

        public List<Participants> GetAll()
        {
            throw new NotImplementedException();
        }

        public Participants GetById(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Participants> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Update(Participants entity)
        {
            throw new NotImplementedException();
        }

        public Participants GetParticipantByConversationIdAndUserId(int conversationId, int userId)
        {
            return ParticipantDAO.Instance.GetParticipantByConversationIdAndUserId(conversationId, userId);
        }

        public void OutConversation(int conversationId, int userId)
        {
            var participant = ParticipantDAO.Instance.GetParticipantByConversationIdAndUserId(conversationId, userId);

            if (participant != null)
            {
                ParticipantDAO.Instance.Remove(participant);
            }
        }

        public bool IsLastAdminInConversation(int conversationId, int userId)
        {
            return ParticipantDAO.Instance
                .GetAll()
                .Count(p => p.ConversationId == conversationId && p.isAdmin) <= 1 &&
                    ParticipantDAO.Instance
                .GetAll()
                .Any(p => p.UserId == userId &&
                          p.ConversationId == conversationId &&
                          p.isAdmin);
        }

        public bool IsLastMemberInConversation(int conversationId)
        {
            int participantCount = ParticipantDAO.Instance
                .GetAll()
                .Count(p => p.ConversationId == conversationId);

            return participantCount == 1;
        }
    }
}
