using InternetBanking.Core.Application.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transactions
{
    public class SaveTransactionViewModel
    {
        public int Id { get; set; } 
        public int AccountId { get; set; } 
        public decimal Amount { get; set; } 
        public TransferType TransactionType { get; set; } 
        public DateTime TransactionDate { get; set; } 
    }
}
