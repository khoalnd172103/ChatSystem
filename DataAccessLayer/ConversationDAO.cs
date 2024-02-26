using BusinessObject;

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
    }
}
