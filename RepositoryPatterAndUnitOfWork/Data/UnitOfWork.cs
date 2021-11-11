using Microsoft.Extensions.Logging;
using RepositoryPatterAndUnitOfWork.Core.IConfiguration;
using RepositoryPatterAndUnitOfWork.Core.IRepository;
using RepositoryPatterAndUnitOfWork.Core.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ILogger _logger;
        private readonly ApplicationDbContext _dbContext;
        public IUserRepository UserRepository { get; }
        public UnitOfWork(ILoggerFactory logger, ApplicationDbContext dbContext,IUserRepository userRepository)
        {
            _logger = logger.CreateLogger("logs");
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            UserRepository = userRepository;
            //UserRepository = new UserRepository(_logger, _dbContext);
        }



        public async Task CompleteAsync()
            => await _dbContext.SaveChangesAsync();


        private bool disposed = false;

        protected async virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    await _dbContext.DisposeAsync();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
