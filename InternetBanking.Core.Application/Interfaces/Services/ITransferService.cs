

using InternetBanking.Core.Application.ViewModels.Transfers;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface ITransferService: IGenericService<SaveTransferViewModel, TransferViewModel, Transfer>
    {

    }
}
