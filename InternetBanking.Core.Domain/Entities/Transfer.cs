

namespace InternetBanking.Core.Domain.Entities
{
    public class Transfer
    {
        public int Id { get; set; }
       // public string UserId { get; set; }



        public int AccountSourceId { get; set; }
        public int DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public DateTime TransferDate { get; set; }

        // Navigation properties for source and destination accounts
        public Account SourceAccount { get; set; }
        public Account DestinationAccount { get; set; }
    }
}
