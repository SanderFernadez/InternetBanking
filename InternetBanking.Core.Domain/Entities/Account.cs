using InternetBanking.Core.Domain.Enums;


namespace InternetBanking.Core.Domain.Entities
{
    public class Account
    {
        public int Id { get; set; }
        public AccountType AccountType { get; set; }

        public int AccountNumber { get; set; }          

        public decimal InitialAmount { get; set; }
        public decimal CurrentBalance { get; set; }
        public string UserId { get; set; }

        public decimal? CreditLimit { get; set; }
        public decimal? LoanAmount { get; set; }


        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Advance> Advances { get; set; }
        public ICollection<Transfer> OriginTransfers { get; set; }   // Correct type
        public ICollection<Transfer> DestinationTransfers { get; set; } // Correct type
    }
}
