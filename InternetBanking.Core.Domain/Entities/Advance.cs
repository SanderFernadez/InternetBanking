

namespace InternetBanking.Core.Domain.Entities
{
    public class Advance
    {
        public int Id { get; set; }

        public int AccountCreditId  { get; set; }
        public int DestinationAccountId { get; set; }
        public decimal Amount { get; set; }
        public decimal Interest { get; set; }
        public DateTime DateAdvance { get; set; }

        public Account CreditAccount { get; set; }
        public Account DestinationAccount { get; set; }


    }
}
