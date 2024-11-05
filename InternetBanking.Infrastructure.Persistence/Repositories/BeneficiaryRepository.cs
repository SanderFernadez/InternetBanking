
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public  class BeneficiaryRepository : GenericRepository<Beneficiary>, IBeneficiaryRepository
    {
        private readonly PersistenceContext _dbContext;

        public BeneficiaryRepository(PersistenceContext dbContext) : base(dbContext)
        {
            _dbContext = dbContext;
        }
    }
}
