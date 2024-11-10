
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.Dtos.BankAccounts
{
    public class UserBankAccouns
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public AccountType AccountType { get; set; }

        public decimal InitialAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public decimal CreditLimit { get; set; }

    }
}
