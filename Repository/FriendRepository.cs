using BusinessObject;
using DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public class FriendRepository : IFriendRepository
    {
        private readonly FriendDAO friendDAO;

        public FriendRepository()
        {
            friendDAO = new FriendDAO();
        }
        public void Create(Friend entity)
        {
            friendDAO.Create(entity);
        }

        public bool Delete(Friend entity)
        {
            throw new NotImplementedException();
        }

        public List<Friend> GetAll()
        {
            throw new NotImplementedException();
        }

        public Friend GetById(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Friend> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public List<Friend> GetFriendRequest(string recipientUserName)
        {
            return friendDAO.GetAll().Where(fr => fr.RecipientUserName == recipientUserName &&
                fr.status == false).ToList();
        }

        public bool Update(Friend entity)
        {
            throw new NotImplementedException();
        }
    }
}
