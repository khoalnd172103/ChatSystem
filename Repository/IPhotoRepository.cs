using BusinessObject;

namespace Repository
{
    public interface IPhotoRepository : IBaseRepository<Photo>
    {
        void CreatePhoto(Photo photo);
        void UpdatePhoto(Photo photo);
        Photo GetUserPhotoIsMain(int userId);
    }
}
