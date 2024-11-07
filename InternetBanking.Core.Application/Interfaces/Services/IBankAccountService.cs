using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IBankAccountService : IGenericService<SaveBankAccountViewModel, BankAccountViewModel, Account>
    {

        int GenerateAccountNumber();
        Task<int> NumberOfProductsClient();

        Task<List<BankAccountViewModel>> GetDatesOfSystem();


        Task<List<BankAccountViewModel>> GetClientProducts(string UserId);

        Task CreateProduct(AccountType accountType, string userId, decimal creditLimit, decimal loanAmount);




        }
}
