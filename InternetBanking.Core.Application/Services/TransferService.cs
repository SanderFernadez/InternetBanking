
using AutoMapper;
using InternetBanking.Core.Application.Dtos.SystemDates;
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



        public async Task<List<BankAccountResponse>> GetDatesOfSystem()
        {
            // Obtener datos de cada entidad
            var products = await _bankAccountService.GetAllViewModel();
            var users = await _accountService.GetAllUsersAsync();
            var payments = await _paymentService.GetAllViewModel();
            var transactions = await _transactionService.GetAllViewModel();

            // Mapear los productos a BankAccountResponse
            var responseList = products.Select(product => new BankAccountResponse
            {
                accounts = products,
                Users = users,            // Asignar lista de usuarios
                Payments = payments,      // Asignar lista de pagos
                Transactions = transactions // Asignar lista de transacciones
            }).ToList();

            return responseList;
        }


    }






}

