using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Dtos.BankAccounts;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IBankAccountService : IGenericService<SaveBankAccountViewModel, BankAccountViewModel, Account>
    {

        int GenerateAccountNumber();
        Task<int> NumberOfProductsClient();

        Task<UserBankAccouns> GetUserAccount(int accountnumber);
        Task<List<BankAccountViewModel>> GetClientProducts(string UserId);

        Task CreateProduct(AccountType accountType, string userId, decimal creditLimit, decimal loanAmount);

        Task<List<BankAccountViewModel>> GetAccounts();

        Task<SaveBankAccountViewModel> AddAmountUser(decimal amount, string UserId);




        }
}
