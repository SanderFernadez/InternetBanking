
namespace InternetBanking.Core.Application.ViewModels.BankAccounts
{
    public class SaveBankAccountViewModel
    {
        public int Id { get; set; }
        public string AccountType { get; set; } 
        public int AccountNumber { get; set; }
        public decimal InitialAmount { get; set; } 
        public string UserId { get; set; }
        public decimal ConfirmInitialAmount { get; set; } // Campo para confirmar el monto inicial
    }
}
