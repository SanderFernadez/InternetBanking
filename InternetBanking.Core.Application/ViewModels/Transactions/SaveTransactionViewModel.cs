using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using System.ComponentModel.DataAnnotations;


namespace InternetBanking.Core.Application.ViewModels.Transactions
{
    public class SaveTransactionViewModel
    {
        public int Id { get; set; }

       
        public int DestinationAccount { get; set; } 
        public decimal Amount { get; set; }

        public int SourceAccount { get; set; }
        public TransferType TransactionType { get; set; } 
        public DateTime TransactionDate { get; set; }

        public ICollection<BankAccountViewModel> accounts { get; set; } = new List<BankAccountViewModel>();
    }
}
