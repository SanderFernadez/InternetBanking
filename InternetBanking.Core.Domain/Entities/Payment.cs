

using InternetBanking.Core.Application.Enums;
using System.Transactions;

namespace InternetBanking.Core.Domain.Entities
{
    public class Payment
    {
        public int Id { get; set; }
        public int TransactionId  { get; set; }
        public int DestinationAccount { get; set; }           
        public Decimal AmountPaid { get; set; }
        public TransferType TransactionType { get; set; }
        public Transaction Transaction { get; set; }

    }
}
