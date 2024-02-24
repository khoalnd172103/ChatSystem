using BusinessObject;
using DataAccessLayer;

namespace Repository
{
    public class PhotoRepository : IPhotoRepository
    {
        public void Create(Photo entity)
        {
            throw new NotImplementedException();
        }

        public void CreatePhoto(Photo photo)
        {
            PhotoDAO.Instance.Create(photo);
        }

        public bool Delete(Photo entity)
        {
            throw new NotImplementedException();
        }

        public List<Photo> GetAll()
        {
            throw new NotImplementedException();
        }

        public Photo GetById(int entityId)
        {
            throw new NotImplementedException();
        }

        public List<Photo> GetByName(string name)
        {
            throw new NotImplementedException();
        }

        public Photo GetUserPhotoIsMain(int userId)
        {
            return PhotoDAO.Instance.GetAll().SingleOrDefault(p => p.UserId == userId && p.isMain == true);
        }

        public bool Update(Photo entity)
        {
            throw new NotImplementedException();
        }

        public void UpdatePhoto(Photo photo)
        {
            PhotoDAO.Instance.Update(photo);
        }
    }
}
