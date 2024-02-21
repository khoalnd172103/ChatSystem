using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository
{
    public interface IBaseRepository<T> where T : class
    {
        public List<T> GetAll();
        public void Create(T entity);
        public bool Delete(T entity);

        public bool Update(T entity);
        public T GetById(int entityId);
        public List<T> GetByName(string name);
    }
}
