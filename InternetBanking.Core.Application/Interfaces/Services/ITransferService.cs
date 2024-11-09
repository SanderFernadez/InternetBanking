

using InternetBanking.Core.Application.Dtos.SystemDates;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Transfers;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ITransferService: IGenericService<SaveTransferViewModel, TransferViewModel, Transfer>
    {
        Task<List<BankAccountResponse>> GetDatesOfSystem();
    }
}
