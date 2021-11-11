using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RepositoryPatterAndUnitOfWork.Core.IRepository;
using RepositoryPatterAndUnitOfWork.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Core.Repository
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        protected readonly ApplicationDbContext _dbContext;
        protected readonly ILogger<GenericRepository<TEntity>> _logger;
        protected DbSet<TEntity> _dbset;
        public GenericRepository(ApplicationDbContext dbContext, ILogger<GenericRepository<TEntity>> logger)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbset = _dbContext.Set<TEntity>();
        }
        public virtual async Task<TEntity> Add(TEntity entity)
        {
            if (entity is null) return null;
            try
            {
                await _dbset.AddAsync(entity);
                return entity;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Add method error", typeof(TEntity));
                return null;
            }
        }

        public virtual Task<bool> Delete(Guid id)
            => throw new NotImplementedException();


        public virtual async Task<IReadOnlyList<TEntity>> GetAll(
               Expression<Func<TEntity, bool>> filter = null,
               Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null)
        {
            IQueryable<TEntity> query = _dbset;

            if (filter is not null) query = query.Where(filter);

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.AsNoTracking().ToListAsync();
            }

        }

        public virtual async Task<TEntity> GetByID(Guid id)
            => await _dbset.FindAsync(id);

        public virtual Task<bool> Upsert(TEntity entity)
            => throw new NotImplementedException();

    }
}
