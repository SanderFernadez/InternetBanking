using AutoMapper; // Asegúrate de tener esta referencia si usas el mapper
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Persistence.Repositories
{
    public class BankAccountRepository : GenericRepository<Account>, IBankAccountRepository
    {
        private readonly PersistenceContext _dbContext;
        private readonly IMapper _mapper;

        public BankAccountRepository(PersistenceContext dbContext, IMapper mapper) : base(dbContext)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

      
    }
}
