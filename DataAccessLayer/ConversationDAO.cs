using BusinessObject;
using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;

namespace DataAccessLayer
{
    public class ConversationDAO : BaseDAO<Conversation>
    {
        private ConversationDAO() { }

        private static ConversationDAO instance = null;
        private static readonly object instacelock = new object();

        public static ConversationDAO Instance
        {
            get
            {
                lock (instacelock)
                {
                    if (instance == null)
                    {
                        instance = new ConversationDAO();
                    }
                    return instance;
                }
            }
        }


        public List<Conversation> GetConverstationsOfUser(int userId)
        {
            List<Conversation> conversations = new List<Conversation>();
            using (var context = new DataContext())
            {
                var participants = context.Participants
                    .Where(p => p.UserId == userId)
                    .ToList();

                foreach (Participants participant in participants)
                {
                    Conversation c = context.Conversations.Include(c => c.MessagesReceived).FirstOrDefault(c => c.ConversationId == participant.ConversationId);
                    conversations.Add(c);
                }
            }
            return conversations;
        }
        public Conversation GetConversationById(int conversationId)
        {
            return GetAll().FirstOrDefault(c => c.ConversationId == conversationId);
        }
        public Conversation GetConversationAndParticipantById(int conversationId)
        {
            Conversation conversation = new Conversation();
            using (var context = new DataContext())
            {
                conversation = context.Conversations.Include(c => c.Participants).FirstOrDefault(c => c.ConversationId == conversationId);
            }
            return conversation;
        }
    }
}
