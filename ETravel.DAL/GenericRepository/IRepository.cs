using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ETravel.DAL
{
    public interface IRepository<TEntity> : IDisposable
    {
        IEnumerable<TEntity> All();
        Task<IEnumerable<TEntity>> AllAsync();

        IEnumerable<TEntity> SearchFor(Expression<Func<TEntity, bool>> predicate);
        Task<IEnumerable<TEntity>> SearchForAsync(Expression<Func<TEntity, bool>> predicate);

        TEntity FindById(long id);
        Task<TEntity> FindByIdAsync(long id);

        void Delete(TEntity entity, bool save = true);
        Task DeleteAsync(TEntity entity, bool save = true);

        void Insert(TEntity entity, bool save = true);
        Task InsertAsync(TEntity entity, bool save = true);

        void Update(TEntity entity, bool save = true);
    }
}
