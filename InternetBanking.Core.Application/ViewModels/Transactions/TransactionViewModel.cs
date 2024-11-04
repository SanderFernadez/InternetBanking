using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transactions
{
    public class TransactionViewModel
    {
        public int Id { get; set; } 
        public int AccountId { get; set; } 
        public decimal Amount { get; set; } 
        public string TransactionType { get; set; } 
        public DateTime TransactionDate { get; set; } 

     
     
    }
}
