using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

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


        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; }
        public ICollection<Advance> Advances { get; set; }
        public ICollection<Transfer> OriginTransfers { get; set; }   // Correct type
        public ICollection<Transfer> DestinationTransfers { get; set; } // Correct type
    }
}
