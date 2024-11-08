

using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentService: IGenericService<SavePaymentViewModel, PaymentViewModel, Payment>
    {
        Task SaveTransaction(SaveTransactionViewModel vm);
        Task UpdateAccounts(SavePaymentViewModel vm);
    }
}
