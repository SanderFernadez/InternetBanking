

using InternetBanking.Core.Application.ViewModels.Advances;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IAdvanceService: IGenericService<SaveAdvanceViewModel, AdvanceViewModel, Advance>
    {
        Task<SaveAdvanceViewModel> CashAdvance(SaveAdvanceViewModel vm);
    }
}
