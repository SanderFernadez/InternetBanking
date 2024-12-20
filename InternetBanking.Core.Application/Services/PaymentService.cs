﻿
using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;

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


        public async Task<SavePaymentViewModel> UpdateCreditAccounts(SavePaymentViewModel vm)
        {
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

            // Verificar si la cuenta de origen es igual a la cuenta de destino, y si es así, transferir 0 pesos
            if (originAccount.AccountNumber == vm.DestinationAccount)
            {
                vm.AmountPaid = 0;  // No realizar ninguna transferencia si son iguales
            }

            // Verificar saldo o crédito disponible en cuenta de origen
            decimal amountToPay = vm.AmountPaid;
            if (originAccount.AccountType == AccountType.Credit)
            {
                decimal availableCredit = (originAccount.CreditLimit ?? 0) - (originAccount.LoanAmount ?? 0);
                if (availableCredit < vm.AmountPaid)
                {
                    return null; // Retornar null si no hay suficiente crédito disponible
                }
                // Aumentar el uso de crédito en la cuenta de origen
                originAccount.LoanAmount += vm.AmountPaid;
            }
            else if (originAccount.CurrentBalance < vm.AmountPaid)
            {
                return null; // Retornar null si el saldo de la cuenta de origen es insuficiente
            }
            else
            {
                originAccount.CurrentBalance -= vm.AmountPaid;
            }

            // Verificar si la cuenta de destino es una tarjeta de crédito o un préstamo
            decimal amountRemaining = 0;
            if (destinationAccount.AccountType == AccountType.Credit)
            {
                // Si el monto pagado es mayor a la deuda, solo toma lo necesario para cubrir la deuda
                decimal debtAmount = destinationAccount.LoanAmount ?? 0;
                amountToPay = Math.Min(vm.AmountPaid, debtAmount); // No se paga más que la deuda
                destinationAccount.LoanAmount -= amountToPay;

                // Si el monto pagado es mayor que la deuda, el sobrante se devuelve a la cuenta de origen
                amountRemaining = vm.AmountPaid - amountToPay;
            }
            else if (destinationAccount.AccountType == AccountType.Loan)
            {
                // Verificar que el monto pagado no exceda el préstamo
                decimal loanAmount = destinationAccount.LoanAmount ?? 0;
                amountToPay = Math.Min(vm.AmountPaid, loanAmount); // No se paga más que lo que se debe
                destinationAccount.LoanAmount -= amountToPay;

                // Si el monto pagado es mayor que el préstamo, el sobrante se devuelve a la cuenta de origen
                amountRemaining = vm.AmountPaid - amountToPay;
            }
            else
            {
                throw new InvalidOperationException("La cuenta destino no es ni una tarjeta de crédito ni un préstamo.");
            }

            // Si hay un sobrante, se devuelve a la cuenta de origen
            if (amountRemaining > 0)
            {
                if (originAccount.AccountType == AccountType.Credit)
                {
                    originAccount.CurrentBalance += amountRemaining; // Devuelve el sobrante al balance de la cuenta de crédito
                }
                else
                {
                    originAccount.CurrentBalance += amountRemaining; // Devuelve el sobrante al balance de la cuenta corriente
                }
            }

            // Crear SaveBankAccountViewModel para actualizar las cuentas en la base de datos
            SaveBankAccountViewModel saveOriginAccount = new SaveBankAccountViewModel()
            {
                Id = originAccount.Id,
                AccountType = originAccount.AccountType,
                AccountNumber = originAccount.AccountNumber,
                InitialAmount = originAccount.InitialAmount,
                UserId = originAccount.UserId,
                CurrentBalance = originAccount.CurrentBalance,
                CreditLimit = originAccount.CreditLimit,
                LoanAmount = originAccount.LoanAmount
            };

            SaveBankAccountViewModel saveDestinationAccount = new SaveBankAccountViewModel()
            {
                Id = destinationAccount.Id,
                AccountType = destinationAccount.AccountType,
                AccountNumber = destinationAccount.AccountNumber,
                InitialAmount = destinationAccount.InitialAmount,
                UserId = destinationAccount.UserId,
                CurrentBalance = destinationAccount.CurrentBalance,
                CreditLimit = destinationAccount.CreditLimit,
                LoanAmount = destinationAccount.LoanAmount
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
            }

            // Registrar el pago en la base de datos
            SavePaymentViewModel payment = new()
            {
                DestinationAccount = vm.DestinationAccount,
                AmountPaid = amountToPay, // Registra solo la cantidad que se usó para pagar la deuda
                SourceAccount = vm.SourceAccount,
                TransactionType = vm.TransactionType,
                PaymentDate = DateTime.Now
            };

            await base.Add(payment);

            return payment;
        }




        public async Task<SavePaymentViewModel> UpdateBeneficiary(SavePaymentViewModel vm)
        {
            // Obtener todas las cuentas
            var allAccounts = await _bankAccountService.GetAllViewModel();

            // Obtener la cuenta de destino (beneficiario) seleccionada en el ViewModel
            var destinationAccount = allAccounts.FirstOrDefault(a => a.AccountNumber == vm.DestinationAccount); // Asegúrate de que el ViewModel tiene una propiedad `BeneficiaryAccount`
            if (destinationAccount == null)
            {
                throw new InvalidOperationException("La cuenta destino no existe.");
            }

            // Obtener la cuenta de origen seleccionada en el ViewModel
            var originAccount = allAccounts.FirstOrDefault(a => a.AccountNumber == vm.SourceAccount);
            if (originAccount == null)
            {
                throw new InvalidOperationException("La cuenta de origen seleccionada no es válida.");
            }

            // Validar que la cuenta de origen tiene suficiente saldo o crédito
            if (originAccount.AccountType == AccountType.Credit)
            {
                decimal creditValue = (originAccount.CreditLimit ?? 0) - (originAccount.LoanAmount ?? 0);
                if (creditValue < vm.AmountPaid)
                {
                    return null; // Retornar null si no hay suficiente crédito disponible
                }

                // Actualizar el saldo de la cuenta de crédito
                originAccount.LoanAmount += vm.AmountPaid;
            }

            else if (originAccount.CurrentBalance < vm.AmountPaid)
            {
                return null; // Retornar null si el saldo de la cuenta de origen es insuficiente
            }


            else
            {
                originAccount.CurrentBalance -= vm.AmountPaid;
            }

            // Actualizar saldo en la cuenta de destino
            destinationAccount.CurrentBalance += vm.AmountPaid;


            // Crear SaveBankAccountViewModel para actualizar las cuentas en la base de datos
            var saveOriginAccount = new SaveBankAccountViewModel
            {
                Id = originAccount.Id,
                AccountType = originAccount.AccountType,
                AccountNumber = originAccount.AccountNumber,
                InitialAmount = originAccount.InitialAmount,
                UserId = originAccount.UserId,
                CurrentBalance = originAccount.CurrentBalance,
                CreditLimit = originAccount.CreditLimit,
                LoanAmount = originAccount.LoanAmount
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
                LoanAmount = destinationAccount.LoanAmount
            };

            // Actualizar las cuentas en la base de datos
            await _bankAccountService.Update(saveOriginAccount, saveOriginAccount.Id);
            await _bankAccountService.Update(saveDestinationAccount, saveDestinationAccount.Id);

            // Registrar el pago en la base de datos
            var payment = new SavePaymentViewModel
            {
                DestinationAccount = vm.DestinationAccount,
                AmountPaid = vm.AmountPaid,
                SourceAccount = vm.SourceAccount,
                TransactionType = vm.TransactionType,
                PaymentDate = DateTime.Now
            };

            await base.Add(payment);

            return payment;
        }



        public async Task<SavePaymentViewModel> UpdateAccounts(SavePaymentViewModel vm)
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

            // Validar que la cuenta de origen tiene suficiente saldo o crédito
            if (originAccount.AccountType == AccountType.Credit)
            {
                decimal creditValue = (originAccount.CreditLimit ?? 0) - (originAccount.LoanAmount ?? 0);
                if (creditValue < vm.AmountPaid)
                {
                    return null; // Retornar null si no hay suficiente crédito disponible
                }

                // Actualizar el saldo de la cuenta de crédito
                originAccount.LoanAmount += vm.AmountPaid;
            }
            else if (originAccount.CurrentBalance < vm.AmountPaid)
            {
                return null; // Retornar null si el saldo de la cuenta de origen es insuficiente
            }
            else
            {
                originAccount.CurrentBalance -= vm.AmountPaid;
            }

            // Actualizar saldo en la cuenta de destino
            destinationAccount.CurrentBalance += vm.AmountPaid;

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
            var payment = new SavePaymentViewModel
            {
                DestinationAccount = vm.DestinationAccount,
                AmountPaid = vm.AmountPaid,
                SourceAccount = vm.SourceAccount,
                TransactionType = vm.TransactionType,
                PaymentDate = DateTime.Now
            };

            await base.Add(payment);

            return payment;
        }







    }
}
