﻿using BusinessObject;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class ConversationRepository : IConversationRepository
    {
        public void Create(Conversation entity)
        {
            ConversationDAO.Instance.Create(entity);
        }

        public void CreateGroup(int creatorId, string groupName, List<string> memberIdList)
        {
            var conversation = new Conversation
            {
                ConversationName = groupName,
                UserId = creatorId,
                CreateAt = DateTime.Now,
                isGroup = true,
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
        }

        public bool Delete(Conversation entity)
        {
            throw new NotImplementedException();
        }

        public List<Conversation> GetAll()
        {
            throw new NotImplementedException();
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
    }
}
