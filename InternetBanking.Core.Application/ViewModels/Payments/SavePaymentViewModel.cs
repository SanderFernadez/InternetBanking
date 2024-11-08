using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.ViewModels.BankAccounts;


namespace InternetBanking.Core.Application.ViewModels.Payments
{
    public class SavePaymentViewModel
    {
        public int Id { get; set; } 
        public int? TransactionId { get; set; }
        public int SourceAccount { get; set; }
        public int DestinationAccount { get; set; } 
        public decimal AmountPaid { get; set; } 
        public DateTime PaymentDate { get; set; }
        public TransferType TransactionType { get; set; }
        public ICollection<BankAccountViewModel> accounts { get; set; }
    }
}
