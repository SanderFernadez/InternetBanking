using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Advances
{
    public class AdvanceViewModel
    {
        public int Id { get; set; }

        public int AccountCreditId { get; set; }
        public int DestinationAccountId { get; set; 
        public decimal Amount { get; set; }
        public decimal Interest { get; set; }
        public DateTime DateAdvance { get; set; }

      

    
    }
}
