using InternetBanking.Core.Application.Enums;


namespace InternetBanking.Core.Application.ViewModels.Payments
{
    public class PaymentViewModel
    {
        public int Id { get; set; } 
        public int TransactionId { get; set; }
        public int SourceAccount { get; set; }
        public int DestinationAccount { get; set; } 
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; } 
        public TransferType TransactionType { get; set; } 

       // public string DestinationAccountName { get; set; } 
    }
}
