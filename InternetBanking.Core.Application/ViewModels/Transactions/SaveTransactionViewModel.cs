using InternetBanking.Core.Application.Enums;


namespace InternetBanking.Core.Application.ViewModels.Transactions
{
    public class SaveTransactionViewModel
    {
        public int Id { get; set; } 
        public int AccountId { get; set; } 
        public decimal Amount { get; set; }

        public int SourceAccount { get; set; }
        public TransferType TransactionType { get; set; } 
        public DateTime TransactionDate { get; set; } 
    }
}
