
using InternetBanking.Core.Domain.Enums;

namespace InternetBanking.Core.Application.ViewModels.BankAccounts
{
    public class SaveBankAccountViewModel
    {
        public int Id { get; set; }
        public AccountType AccountType { get; set; }
        public int AccountNumber { get; set; }
        public decimal InitialAmount { get; set; } 
        public string UserId { get; set; }
        public decimal CurrentBalance { get; set; } 
    }
}
