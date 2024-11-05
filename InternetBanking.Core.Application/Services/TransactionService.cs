

using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Entities;


namespace InternetBanking.Core.Application.Services
{
    internal class TransactionService: GenericService<SaveTransactionViewModel, TransactionViewModel, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IMapper _mapper;

        public TransactionService (ITransactionRepository transactionRepository, IMapper mapper): base(transactionRepository,mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
        }
    }
}
