using BusinessObject;
using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;

namespace DataAccessLayer
{
    public class ParticipantDAO : BaseDAO<Participants>
    {

        public ParticipantDAO() { }

        private static ParticipantDAO instance = null;
        private static readonly object instacelock = new object();

        public static ParticipantDAO Instance
        {
            get
            {
                lock (instacelock)
                {
                    if (instance == null)
                    {
                        instance = new ParticipantDAO();
                    }
                    return instance;
                }
            }
        }

        public User GetOtherParticipantWhenNotGroupConversation(int conversationId, int userId)
        {
            User otherUser = null;
            using (var context = new DataContext())
            {
                Participants otherParticipant = context.Participants.FirstOrDefault(p => conversationId == p.ConversationId && p.UserId != userId);

                otherUser = context.Users.Include(u => u.photos).FirstOrDefault(u => u.UserId == otherParticipant.UserId);
            }
            return otherUser;

        }

        public Participants GetParticipantByConversationIdAndUserId(int conversationId, int userId)
        {
            Participants participants = null;
            using (var context = new DataContext())
            {
                participants = context.Participants
                .SingleOrDefault(p => p.ConversationId == conversationId && p.UserId == userId);
            }
            return participants;
        }

        public List<Participants> GetParticipantsByConversationId(int conversationId)
        {
            var context = new DataContext();
            return context.Participants.Where(p => p.ConversationId == conversationId).ToList();
        }
    }
}
