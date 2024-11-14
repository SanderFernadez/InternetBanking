
using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Advances;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.Services
{
    public class AdvanceService: GenericService<SaveAdvanceViewModel, AdvanceViewModel, Advance>, IAdvanceService
    {
        private readonly IAdvanceRepository advanceRepository;
        private readonly IBankAccountService _bankAccountService;
        private readonly IMapper _mapper;

        public AdvanceService( IBankAccountService bankAccountService, IAdvanceRepository advanceRepository, IMapper mapper): base(advanceRepository,mapper)
        {
            this.advanceRepository = advanceRepository;
            _bankAccountService = bankAccountService;
            _mapper = mapper;
        }





        public async Task<SaveAdvanceViewModel> CashAdvance(SaveAdvanceViewModel vm)
        {
            // Obtener todas las cuentas para validar la cuenta de destino
            var allAccounts = await _bankAccountService.GetAllViewModel();

            // Validar que la cuenta destino existe
            var destinationAccount = allAccounts.FirstOrDefault(a => a.AccountNumber == vm.DestinationAccountId);
            if (destinationAccount == null)
            {
                throw new InvalidOperationException("La cuenta destino no existe.");
            }

            // Obtener la cuenta de origen seleccionada en el ViewModel
            var originAccount = vm.accounts.FirstOrDefault(a => a.Id == vm.AccountCreditId);
            if (originAccount == null)
            {
                throw new InvalidOperationException("La cuenta de origen seleccionada no es válida.");
            }

            // Validación para avances de efectivo con tarjeta de crédito
            if (originAccount.AccountType == AccountType.Credit)
            {
                decimal availableCredit = (originAccount.CreditLimit ?? 0) - (originAccount.LoanAmount ?? 0);

                // Verificar si el avance de efectivo no supera el límite de crédito disponible
                if (availableCredit < vm.Amount)
                {
                    return null; 
                }

                // Calcular el monto de la deuda con el 6.25% de interés adicional
                decimal interestRate = 0.0625m;
                decimal debtAmount = vm.Amount * (1 + interestRate);

                // Actualizar la deuda en la cuenta de crédito (LoanAmount)
                originAccount.LoanAmount += debtAmount;

                // Actualizar saldo de la cuenta destino
                destinationAccount.CurrentBalance += vm.Amount;
            }
            else
            {
                // Validar que la cuenta de origen tiene saldo suficiente si no es de crédito
                if (originAccount.CurrentBalance < vm.Amount)
                {
                    throw new InvalidOperationException("Saldo insuficiente en la cuenta de origen.");
                }

                // Descontar el monto de la cuenta de origen
                originAccount.CurrentBalance -= vm.Amount;

                // Actualizar saldo en la cuenta destino
                destinationAccount.CurrentBalance += vm.Amount;
            }

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
                Console.WriteLine($"Error actualizando cuentas: {ex.Message}");
                throw;
            }

            // Registrar la transacción en la base de datos
            var transaction = new SaveAdvanceViewModel
            {
                DestinationAccountId = destinationAccount.Id,
                Amount = vm.Amount,
                AccountCreditId = saveOriginAccount.Id,
                DateAdvance = DateTime.Now,
                Interest = 6.25M,
            }; 

            await base.Add(transaction);

            return transaction;
        }














    }
}
