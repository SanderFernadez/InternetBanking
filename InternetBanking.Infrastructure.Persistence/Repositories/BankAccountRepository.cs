using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;


namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class BankAccountRepository: GenericRepository<Account>, IBankAccountRepository
    {
        private readonly PersistenceContext _dbContext;

        public BankAccountRepository(PersistenceContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
