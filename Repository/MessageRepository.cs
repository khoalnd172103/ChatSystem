using BusinessObject;

namespace Repository
{
    public class MessageRepository : IMessageRepository
    {
        public void Create(Message entity)
        {
            throw new NotImplementedException();
        }

        public bool Delete(Message entity)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetAll()
        {
            throw new NotImplementedException();
        }

        public Message GetById(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public bool Update(Message entity)
        {
            throw new NotImplementedException();
        }

        public Message GetLastestMessageFromConversation(Conversation conversation)
        {
            if (conversation.MessagesReceived.Count == 0)
            {
                return new Message
                {
                    DateSend = DateTime.MinValue
                };
            }
            DateTime lastest = DateTime.MinValue;
            foreach (Message message in conversation.MessagesReceived)
            {
                if (message.DateSend.CompareTo(lastest) > 0)
                {
                    lastest = message.DateSend;
                }
            }

            return conversation.MessagesReceived.FirstOrDefault(m => m.DateSend == lastest);
        }
    }
}
