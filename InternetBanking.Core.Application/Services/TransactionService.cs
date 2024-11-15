

using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;


namespace InternetBanking.Core.Application.Services
{
    internal class TransactionService: GenericService<SaveTransactionViewModel, TransactionViewModel, Transaction>, ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IBankAccountService _bankAccountService;
        private readonly IMapper _mapper;
        public TransactionService (IBankAccountService bankAccountService, ITransactionRepository transactionRepository, IMapper mapper): base(transactionRepository,mapper)
        {
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _bankAccountService = bankAccountService;
        }







        public async Task<SaveTransactionViewModel> UpdateAccounts(SaveTransactionViewModel vm)
        {
            // Obtener todas las cuentas para validar la cuenta de destino
            var allAccounts = await _bankAccountService.GetAllViewModel();

            // Validar que la cuenta destino existe
            var destinationAccount = allAccounts.FirstOrDefault(a => a.AccountNumber == vm.DestinationAccount);
            if (destinationAccount == null)
            {
                throw new InvalidOperationException("La cuenta destino no existe.");
            }

            // Obtener la cuenta de origen seleccionada en el ViewModel
            var originAccount = vm.accounts.FirstOrDefault(a => a.Id == vm.SourceAccount);
            if (originAccount == null)
            {
                throw new InvalidOperationException("La cuenta de origen seleccionada no es válida.");
            }

            // Si la cuenta de origen es igual a la cuenta destino, transferir 0 pesos
            if (originAccount.AccountNumber == vm.DestinationAccount)
            {
                vm.Amount = 0;
            }

            // Validar que la cuenta de origen tiene suficiente saldo o crédito
            if (originAccount.AccountType == AccountType.Credit)
            {
                decimal creditValue = (originAccount.CreditLimit ?? 0) - (originAccount.LoanAmount ?? 0);
                if (creditValue < vm.Amount)
                {
                    return null; // Retornar null si no hay suficiente crédito disponible
                }

                // Actualizar el saldo de la cuenta de crédito
                originAccount.LoanAmount += vm.Amount;
            }
            else if (originAccount.CurrentBalance < vm.Amount)
            {
                return null; // Retornar null si el saldo de la cuenta de origen es insuficiente
            }
            else
            {
                originAccount.CurrentBalance -= vm.Amount;
            }

            // Actualizar saldo en la cuenta de destino
            destinationAccount.CurrentBalance += vm.Amount;

            // Crear modelos para actualizar en la base de datos
            var saveOriginAccount = new SaveBankAccountViewModel
            {
                Id = originAccount.Id,
                AccountType = originAccount.AccountType,
                AccountNumber = originAccount.AccountNumber,
                InitialAmount = originAccount.InitialAmount,
                UserId = originAccount.UserId,
                CurrentBalance = originAccount.CurrentBalance,
                CreditLimit = originAccount.CreditLimit,
                LoanAmount = originAccount.LoanAmount,
            };

            var saveDestinationAccount = new SaveBankAccountViewModel
            {
                Id = destinationAccount.Id,
                AccountType = destinationAccount.AccountType,
                AccountNumber = destinationAccount.AccountNumber,
                InitialAmount = destinationAccount.InitialAmount,
                UserId = destinationAccount.UserId,
                CurrentBalance = destinationAccount.CurrentBalance,
                CreditLimit = destinationAccount.CreditLimit,
                LoanAmount = destinationAccount.LoanAmount,
            };

            // Actualizar las cuentas en la base de datos
            try
            {
                await _bankAccountService.Update(saveOriginAccount, saveOriginAccount.Id);
                await _bankAccountService.Update(saveDestinationAccount, saveDestinationAccount.Id);
            }
            catch (Exception ex)
            {
                // Manejo de errores en la actualización
                Console.WriteLine($"Error actualizando cuentas: {ex.Message}");
                throw; // Lanza la excepción para manejarla en niveles superiores
            }

            // Registrar el pago en la base de datos
            var transaction = new SaveTransactionViewModel
            {
                DestinationAccount = destinationAccount.Id,
                Amount = vm.Amount,
                SourceAccount = vm.SourceAccount,
                TransactionType = vm.TransactionType,
                TransactionDate = DateTime.Now
            };

            await base.Add(transaction);

            return transaction;
        }
















    }
}
