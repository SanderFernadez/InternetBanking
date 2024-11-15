
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.ViewModels.BankAccounts;

namespace InternetBanking.Core.Application.Interfaces.Repositories
{
    public interface IBankAccountRepository: IGenericRepository<Account>
    {
     
    }
}
