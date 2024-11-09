
using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Transfers;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class TransferService: GenericService<SaveTransferViewModel, TransferViewModel, Transfer>, ITransferService
    {
        private readonly ITransferRepository _transferRepository;
        private readonly IBankAccountService _bankAccountService;
        private readonly IPaymentService _paymentService;
        private readonly IAccountService _accountService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;

        public TransferService(ITransactionService transactionService, IAccountService accountService, IPaymentService paymentService,IBankAccountService bankAccountService, ITransferRepository transferRepository, IMapper mapper) :base(transferRepository, mapper)
        {
            _transferRepository = transferRepository;
            _bankAccountService = bankAccountService;
            _accountService = accountService;
            _paymentService = paymentService;
            _transactionService = transactionService;
            _mapper = mapper;
        }



        public async Task<List<BankAccountViewModel>> GetDatesOfSystem()
        {
            var products = await _bankAccountService.GetAllViewModel();
            var users = await _accountService.GetAllUsersAsync();
            var payments = await _paymentService.GetAllViewModel();
            var transactions = await _transactionService.GetAllViewModel();

            foreach (var activeUsers in products)
            {
                activeUsers.Users = users;
                activeUsers.Payments = payments; 
                activeUsers.Transactions = transactions;
            }

            return products;
        }






    }
}
