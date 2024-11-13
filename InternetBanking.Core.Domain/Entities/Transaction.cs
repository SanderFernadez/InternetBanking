
using InternetBanking.Core.Application.Enums;

namespace InternetBanking.Core.Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public int DestinationAccount { get; set; }
        public decimal Amount { get; set; }
        public TransferType TransactionType { get; set; }
        public int SourceAccount { get; set; }
        public DateTime TransactionDate { get; set; }

        // Relación muchos a uno con Cuenta
        public Account Account { get; set; }

       

    }
}
