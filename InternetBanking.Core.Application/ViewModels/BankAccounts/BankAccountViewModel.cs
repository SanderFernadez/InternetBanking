

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

    }
}
