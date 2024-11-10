using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.ViewModels.BankAccounts
{
    public class BankAccountViewModel
    {
        public int Id { get; set; }

        public AccountType AccountType { get; set; }

        public int AccountNumber { get; set; }

        public Decimal InitialAmount { get; set; }

        public Decimal CurrentBalance { get; set; }

        public string UserId { get; set; }

        public Decimal? CreditLimit { get; set; }
        public Decimal? LoanAmount { get; set; }

      

    }
}
