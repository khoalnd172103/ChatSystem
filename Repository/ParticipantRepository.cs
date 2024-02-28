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
    }
}
