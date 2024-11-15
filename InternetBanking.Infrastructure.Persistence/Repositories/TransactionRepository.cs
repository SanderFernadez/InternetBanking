
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;


namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class TransactionRepository: GenericRepository<Transaction>, ITransactionRepository

    {
        private readonly PersistenceContext _dbContext;

        public TransactionRepository (PersistenceContext dbContext) : base (dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
