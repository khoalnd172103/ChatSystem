using BusinessObject;
using DataAccessLayer;
using Repository.DTOs;

namespace Repository
{
    public class MessageRepository : IMessageRepository
    {
        public void Create(Message entity)
        {
            MessageDAO.Instance.Create(entity);
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

        public List<MessageDto> GetMessagesFromConversation(Conversation conversation, List<UserDto> userDtos)
        {
            List<MessageDto> messageDtos = new List<MessageDto>();

            List<Message> messages = MessageDAO.Instance.GetMessages(conversation.ConversationId);

            foreach (var message in messages)
            {
                if (!message.SenderDelete)
                {
                    UserDto sender = userDtos.FirstOrDefault(u => u.UserId == message.SenderId);
                    MessageDto messageDto = new MessageDto
                    {
                        MessageId = message.MessageId,
                        SenderId = message.SenderId,
                        SenderUserName = sender.UserName,
                        Content = message.Content,
                        DateSend = message.DateSend.ToLocalTime(),
                        DateRead = message.DateRead,
                        SenderDelete = message.SenderDelete,
                        Avatar = sender.Avatar,
                    };

                    TimeSpan timeSpan = DateTime.Now.ToLocalTime() - messageDto.DateSend;

                    messageDto.DisplaySendTime = (int)(timeSpan.TotalMinutes) + " minute(s) ago";
                    if (timeSpan.TotalMinutes > 60)
                    {
                        if (timeSpan.TotalHours > 24)
                            messageDto.DisplaySendTime = (int)(timeSpan.TotalDays) + " day(s) ago";
                        else
                            messageDto.DisplaySendTime = (int)(int)(timeSpan.TotalHours) + " hour(s) ago";
                    }
                    if (timeSpan.TotalMinutes < 1)
                    {
                        messageDto.DisplaySendTime = "recently";
                    }

                    messageDtos.Add(messageDto);
                }
            }

            return messageDtos.OrderByDescending(m => m.DateSend).ToList();
        }
    }
}
