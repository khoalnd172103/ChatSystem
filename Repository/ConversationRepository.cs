using BusinessObject;
using DataAccessLayer;

namespace Repository
{
    public class ConversationRepository : IConversationRepository
    {
        public void Create(Conversation entity)
        {
            ConversationDAO.Instance.Create(entity);
        }

        public Conversation CreateGroup(int creatorId, string groupName, List<string> memberIdList)
        {
            var conversation = new Conversation
            {
                ConversationName = groupName,
                UserId = creatorId,
                CreateAt = DateTime.Now,
                isGroup = true,
                MessagesReceived = new List<Message>(),
                Participants = new List<Participants>()
            };

            conversation.Participants.Add(new Participants
            {
                UserId = creatorId,
                status = 1,
                isAdmin = true,
                Conversation = conversation
            });

            conversation.Participants.AddRange(memberIdList.Select(friendId => new Participants
            {
                UserId = int.Parse(friendId),
                status = 1,
                isAdmin = false,
                Conversation = conversation
            }));

            ConversationDAO.Instance.Create(conversation);

            return conversation;
        }

        public void AddUserToGroup(int creatorId, int conversationId, List<string> memberIdList)
        {
            var currentConversation = ConversationDAO.Instance.GetConversationById(conversationId);

            var conversation = new Conversation
            {
                ConversationId = conversationId,
                ConversationName = currentConversation.ConversationName,
                UserId = creatorId,
                CreateAt = DateTime.Now,
                isGroup = true,
                Participants = new List<Participants>()
            };

            var existingParticipants = ParticipantDAO.Instance.GetParticipantsByConversationId(conversationId);

            foreach (var memberId in memberIdList)
            {
                var friendId = int.Parse(memberId);

                var existingParticipant = existingParticipants.FirstOrDefault(p => p.UserId == friendId && p.status == 0);

                if (existingParticipant != null)
                {
                    existingParticipant.status = 1;
                    ParticipantDAO.Instance.Update(existingParticipant);
                }
                else
                {
                    conversation.Participants.Add(new Participants
                    {
                        UserId = friendId,
                        status = 1,
                        isAdmin = false,
                        Conversation = conversation
                    });
                }
            }

            ConversationDAO.Instance.Update(conversation);
        }


        public bool Delete(Conversation entity)
        {
            throw new NotImplementedException();
        }

        public List<Conversation> GetAll()
        {
            return (List<Conversation>)ConversationDAO.Instance.GetAll();
        }

        public List<Conversation> GetAllUserConversation(int userId)
        {
            List<Conversation> list = ConversationDAO.Instance.GetConverstationsOfUser(userId);
            return list;
        }

        public Conversation GetById(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Conversation> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Conversation GetConversationById(int conversationId)
        {
            return ConversationDAO.Instance.GetConversationById(conversationId);
        }

        public bool Update(Conversation entity)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Conversation>> GetAllConversationById(int userID)
        {
            return await ConversationDAO.Instance.GetUserGroupConversationsByUserId(userID);
        }

        public bool IsUserInConversation(int conversationId, int userId)
        {
            bool result = false;

            Conversation conversation = ConversationDAO.Instance.GetConversationAndParticipantById(conversationId);

            foreach (var participant in conversation.Participants)
            {
                if (participant.UserId == userId)
                {
                    return true;
                }
            }

            return result;
        }

        public void UpdateConversation(Conversation conversation)
        => ConversationDAO.Instance.Update(conversation);
        
        public void DeleteConversation(int conversationId)
        => ConversationDAO.Instance.DeleteConversation(conversationId);

        public Conversation GetConversationBySenderIdAndReceiverId(int senderId, int receiverId)
        {
            return ConversationDAO.Instance.GetConversationBySenderIdAndReceiverId(senderId, receiverId);
        }
    }
}
