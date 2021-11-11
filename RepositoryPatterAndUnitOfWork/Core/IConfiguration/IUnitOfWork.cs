using RepositoryPatterAndUnitOfWork.Core.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RepositoryPatterAndUnitOfWork.Core.IConfiguration
{
    public interface IUnitOfWork:IDisposable
    {
        public IUserRepository UserRepository { get; }

        Task CompleteAsync();
    }
}
