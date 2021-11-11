using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RepositoryPatterAndUnitOfWork.Core.IRepository;
using RepositoryPatterAndUnitOfWork.Data;
using RepositoryPatterAndUnitOfWork.Domain.Entities;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Core.Repository
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        #region Second Way If You Want To Resolve The Register Services Manuel 
        //static IHost host = Host.CreateDefaultBuilder()
        //      .ConfigureWebHostDefaults(webBuilder =>
        //      {
        //          webBuilder.UseStartup<Startup>();
        //      }).Build();
        //static readonly IServiceProvider _serviceProvider = host.Services.CreateScope().ServiceProvider;
        //static ApplicationDbContext dbContext = _serviceProvider.GetRequiredService<ApplicationDbContext>();
        //static ILogger logger = _serviceProvider.GetRequiredService<ILogger<UserRepository>>();

        //public UserRepository() : base(dbContext, logger)
        //{
        //}


        #endregion
        public UserRepository(ILogger<UserRepository> logger, ApplicationDbContext dbContext) : base(dbContext, logger)
        {
        }

        public async override Task<User> GetByID(Guid id)
        {
            var entityFromDb = await _dbset.FindAsync(id);
            return entityFromDb is not null ? entityFromDb : null;
        }

        public async override Task<bool> Upsert(User entity)
        {
            if (entity is null) return false;
            try
            {
                var updatedEntity = await _dbset.AsNoTracking().FirstOrDefaultAsync(x => x.Id == entity.Id);
                if (updatedEntity is null)
                    await _dbset.AddAsync(entity);
                _dbContext.Entry(entity).State = EntityState.Modified;
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Upsert method error", typeof(UserRepository));
                return false;
            }
        }

        public async override Task<bool> Delete(Guid id)
        {
            var deletedEntity = await _dbset.FindAsync(id);
            if (deletedEntity is null) return false;
            try
            {
                _dbset.Remove(deletedEntity);
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Repo} Delete method error", typeof(UserRepository));
                return false;
            }
        }


    }
}
