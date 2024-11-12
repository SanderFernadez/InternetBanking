

using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentService: IGenericService<SavePaymentViewModel, PaymentViewModel, Payment>
    {
        Task<SavePaymentViewModel> UpdateCreditAccounts(SavePaymentViewModel vm);
        Task<SavePaymentViewModel> UpdateAccounts(SavePaymentViewModel vm);

        Task<SavePaymentViewModel> UpdateBeneficiary(SavePaymentViewModel vm);
    }
}
