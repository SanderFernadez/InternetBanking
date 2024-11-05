using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class AdvanceRepository: GenericRepository<Advance>, IAdvanceRepository
    {
        private readonly PersistenceContext _dbContext;

        public AdvanceRepository(PersistenceContext dbContext): base(dbContext) 
        {
            _dbContext = dbContext;
        }   
    }
}
