

using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IPaymentService: IGenericService<SavePaymentViewModel, PaymentViewModel, Payment>
    {

    }
}
