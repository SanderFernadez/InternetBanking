
using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Dtos.SystemDates;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;
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



        public async Task<BankAccountResponse> GetSystemReport()
        {
            var products = await _bankAccountService.GetAllViewModel();
            var users = await _accountService.GetAllUsersAsync();
            var payments = await _paymentService.GetAllViewModel();
            var transactions = await _transactionService.GetAllViewModel();

            // Filtrar las transacciones y pagos del día actual
            var today = DateTime.Now.Date;





            var dailyTransactions = transactions?.Where(t => t.TransactionDate.Date == today).ToList() ?? new List<TransactionViewModel>();
            var dailyPayments = payments?.Where(p => p.PaymentDate.Date == today).ToList() ?? new List<PaymentViewModel>();

            // Contar los usuarios inactivos
            var inactiveUsersCount = users?.Count(u => !u.IsVerified && u.Roles.Any(role => role == Roles.Client.ToString())) ?? 0;
            var activeUsersCount = users?.Count(u => u.IsVerified && u.Roles.Any(role => role == Roles.Client.ToString())) ?? 0;

            // Crear un informe general con el conteo de cada entidad
            var report = new BankAccountResponse
            {
                TotalAccounts = products?.Count ?? 0,
                TotalUsers = activeUsersCount,
                TotalPayments = payments?.Count ?? 0,
                TotalTransactions = transactions?.Count ?? 0,
                DailyTransactionsCount = dailyTransactions.Count,
                DailyPaymentsCount = dailyPayments.Count,
                InactiveUsersCount = inactiveUsersCount
            };

            return report;
        }



    }






}

