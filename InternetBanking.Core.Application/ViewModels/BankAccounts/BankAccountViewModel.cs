

using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.ViewModels.BankAccounts
{
    public class BankAccountViewModel
    {
        public int Id { get; set; } 

        public AccountType AccountType { get; set; } 

        public int AccountNumber { get; set; } 

        public decimal InitialAmount { get; set; } 

        public decimal CurrentBalance { get; set; } 

        public string UserId { get; set; } 

        public ICollection<AuthenticationResponse> Users { get; set; }
        public ICollection<PaymentViewModel> Payments { get; set; } 
        public ICollection<TransactionViewModel> Transactions { get; set; }

    }
}
