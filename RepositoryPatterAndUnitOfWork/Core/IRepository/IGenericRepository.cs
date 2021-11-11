using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Core.IRepository
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        Task<IReadOnlyList<TEntity>> GetAll(
               Expression<Func<TEntity, bool>> filter = null,
               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null);

        Task<TEntity> GetByID(Guid id);

        Task<TEntity> Add(TEntity entity);

        Task<bool> Delete(Guid id);

        Task<bool> Upsert(TEntity entity);




    }
}
