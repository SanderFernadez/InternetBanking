
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ITransactionService : IGenericService<SaveTransactionViewModel, TransactionViewModel, Transaction> 
    {

    }
}
