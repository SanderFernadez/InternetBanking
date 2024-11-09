﻿
using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel, PaymentViewModel, Payment>, IPaymentService 

    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IBankAccountService _bankAccountService;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper, IBankAccountService bankAccountService ) : base( paymentRepository,mapper) 
        {
            _paymentRepository = paymentRepository;
            _bankAccountService = bankAccountService;
            _mapper = mapper;
        }





        public async Task UpdateAccounts(SavePaymentViewModel vm)
        {
            // Obtener todas las cuentas para validar la cuenta de destino
            var allAccounts = await _bankAccountService.GetAllViewModel();

            // Validar que la cuenta destino existe
            var destinationAccount = allAccounts.FirstOrDefault(a => a.AccountNumber == vm.DestinationAccount);
            if (destinationAccount == null)
            {
                throw new InvalidOperationException("La cuenta destino no existe.");
            }

            // Obtener la cuenta de origen seleccionada en el ViewModel (desde la lista de cuentas del usuario)
            var originAccount = vm.accounts.FirstOrDefault(a => a.Id == vm.SourceAccount);
            if (originAccount == null)
            {
                throw new InvalidOperationException("La cuenta de origen seleccionada no es válida.");
            }

            // Validar que la cuenta de origen tiene suficiente saldo
            if (originAccount.CurrentBalance < vm.AmountPaid)
            {
                throw new InvalidOperationException("Ustes no tiene suficiente dinero en su cuenta");
            }

            // Realizar la transacción si todas las validaciones pasaron
            originAccount.CurrentBalance -= vm.AmountPaid;
            destinationAccount.CurrentBalance += vm.AmountPaid;

            // Crear SaveBankAccountViewModel para actualizar las cuentas en la base de datos
            SaveBankAccountViewModel saveoriginAccount = new SaveBankAccountViewModel()
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

            SaveBankAccountViewModel savedestinationAccount = new SaveBankAccountViewModel()
            {
                Id = destinationAccount.Id, // Asegurarse de incluir el Id aquí
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
                await _bankAccountService.Update(saveoriginAccount, saveoriginAccount.Id);
                await _bankAccountService.Update(savedestinationAccount, savedestinationAccount.Id);
            }
            catch (Exception ex)
            {
                // Log o ver detalles del error
                Console.WriteLine($"Error actualizando cuentas: {ex.Message}");
            }


            // Registrar el pago en la base de datos

            SavePaymentViewModel payment = new()
            {
                
                DestinationAccount = vm.DestinationAccount,
                AmountPaid = vm.AmountPaid,
                SourceAccount = vm.SourceAccount,
                TransactionType = vm.TransactionType,
                PaymentDate = DateTime.Now
            };

            await base.Add(payment);


        }

        ////public async Task SaveTransaction(SaveTransactionViewModel vm)
        ////{
            

        ////    var paymentRecord = new SaveTransactionViewModel
        ////    {
        ////        SourceAccount = vm.SourceAccount,
        ////        AccountId = vm.Id,
        ////        Amount = vm.Amount,
        ////        TransactionDate = DateTime.Now,
        ////        TransactionType = vm.TransactionType
        ////    };

        ////    await _transactionService.Add(paymentRecord);


        ////}




    }
}
