
using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Transfers;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class TransferService: GenericService<SaveTransferViewModel, TransferViewModel, Transfer>, ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IMapper _mapper;

        public TransferService(ITransferRepository transferRepository, IMapper mapper) :base(transferRepository, mapper)
        {
            _transferRepository = transferRepository;
            _mapper = mapper;
        }
    }
}
