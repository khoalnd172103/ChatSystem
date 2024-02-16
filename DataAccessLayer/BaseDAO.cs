using Microsoft.EntityFrameworkCore;
using PRN221ProjectGroup.Data;

namespace DataAccessLayer
{
    public class BaseDAO<T> where T : class
    {
        public IEnumerable<T> GetAll()
        {
            List<T> list;
            try
            {
                var _context = new DataContext();
                DbSet<T> _dbSet = _context.Set<T>();
                list = _dbSet.ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return list;
        }

        public void Create(T entity)
        {
            try
            {
                var _context = new DataContext();
                DbSet<T> _dbSet = _context.Set<T>();
                _dbSet.Add(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Update(T entity)
        {
            try
            {
                var _context = new DataContext();
                DbSet<T> _dbSet = _context.Set<T>();
                var tracker = _context.Attach(entity);
                tracker.State = EntityState.Modified;
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Remove(T entity)
        {
            try
            {
                var _context = new DataContext();
                DbSet<T> _dbSet = _context.Set<T>();
                _dbSet.Remove(entity);
                _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
