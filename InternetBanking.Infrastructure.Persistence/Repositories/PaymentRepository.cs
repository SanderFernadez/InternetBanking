﻿

using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class PaymentRepository: GenericRepository<Payment>, IPaymentRepository
    {
        private readonly PersistenceContext _dbcontext;

        public PaymentRepository(PersistenceContext dbcontext) : base(dbcontext) { 
        
             _dbcontext = dbcontext;

        }
    }
}
