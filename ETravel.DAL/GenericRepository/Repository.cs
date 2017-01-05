using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETravel.DAL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected DbSet dbSet;

        private readonly ETravelEntities _context;

        public Repository(ETravelEntities context = null)
        {
            _context = context ?? new ETravelEntities();

            dbSet = _context.Set<TEntity>();
        }

        public IEnumerable<TEntity> All()
        {
            try
            {
                return _context.Set<TEntity>().ToList();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<TEntity>> AllAsync()
        {
            try
            {
                return await _context.Set<TEntity>().ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
          
        }

        public void Delete(TEntity entity, bool save = true)
        {
            try
            {
                _context.Set<TEntity>().Remove(entity);
                
                if (save)
                    _context.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public async Task DeleteAsync(TEntity entity, bool save = true)
        {
            _context.Set<TEntity>().Remove(entity);

            if (save)
              await _context.SaveChangesAsync();
        }
        
        public TEntity FindById(long id)
        {
            try
            {
              return _context.Set<TEntity>().Find(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public async Task<TEntity> FindByIdAsync(long id)
        {
            try
            {
                return await _context.Set<TEntity>().FindAsync(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Insert(TEntity entity, bool save = true)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);

                if (save)
                    _context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        public async Task InsertAsync(TEntity entity, bool save = true)
        {
            try
            {
                _context.Set<TEntity>().Add(entity);

                if (save)
                   await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        public IEnumerable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return _context.Set<TEntity>().Where(predicate).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        public async Task<IEnumerable<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                return await _context.Set<TEntity>().Where(predicate).ToListAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void Update(TEntity entity, bool save = true)
        {
            dbSet.Attach(entity);

            _context.Entry<TEntity>(entity).State = EntityState.Modified;

            if (save)
                _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
