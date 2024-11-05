using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IBankAccountService : IGenericService<SaveBankAccountViewModel, BankAccountViewModel, Account>
    {

        int GenerateAccountNumber();
        Task<int> NumberOfProductsClient();

        Task<List<BankAccountViewModel>> GetDatesOfSystem();

       
    }
}
