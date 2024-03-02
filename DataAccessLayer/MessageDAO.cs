using BusinessObject;
using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;

namespace DataAccessLayer
{
    public class MessageDAO : BaseDAO<Message>
    {
        private MessageDAO() { }

        private static MessageDAO instance = null;
        private static readonly object instacelock = new object();

        public static MessageDAO Instance
        {
            get
            {
                lock (instacelock)
                {
                    if (instance == null)
                    {
                        instance = new MessageDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Message> GetMessages(int conversationId)
        {
            using (var context = new DataContext())
            {
                var messages = context.Messages
                    .Include(m => m.Conversation)
                    .Where(p => p.Conversation.ConversationId == conversationId)
                    .ToList();


                return messages;
            }
        }
    }
}
