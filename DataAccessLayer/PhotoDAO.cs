using BusinessObject;

namespace DataAccessLayer
{
    public class PhotoDAO : BaseDAO<Photo>
    {
        private PhotoDAO() { }

        private static PhotoDAO instance = null;
        private static readonly object instacelock = new object();

        public static PhotoDAO Instance
        {
            get
            {
                lock (instacelock)
                {
                    if (instance == null)
                    {
                        instance = new PhotoDAO();
                    }
                    return instance;
                }
            }
        }
    }
}
