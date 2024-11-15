
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class TransferRepository: GenericRepository<Transfer>, ITransferRepository
    {
        private readonly PersistenceContext _dbcontext;
        public TransferRepository( PersistenceContext dbContext) : base(dbContext)
        {
            _dbcontext = dbContext;
        }
    }
}
