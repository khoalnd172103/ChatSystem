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


        public async Task<List<Conversation>> GetConverstationsOfUser(int userId)
        {
            List<Conversation> conversations = new List<Conversation>();
            using (var context = new DataContext())
            {
                var participants = await context.Participants
                    .Where(p => p.UserId == userId)
                    .ToListAsync();

                foreach (Participants participant in participants)
                {
                    Conversation c = context.Conversations.FirstOrDefault(c => c.ConversationId == participant.ConversationId);
                    conversations.Add(c);
                }
            }
            return conversations;
        }
    }
}
