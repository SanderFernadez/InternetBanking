using InternetBanking.Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Advance
{
    public class AdvanceViewModel
    {
        public int Id { get; set; }

        public int AccountCreditId { get; set; }
        public string AccountCreditName { get; set; } // Nombre de la cuenta de crédito (opcional, para mostrar en la vista
        public int DestinationAccountId { get; set; }
        public string DestinationAccountName { get; set; } // Nombre de la cuenta de destino (opcional, para mostrar en la vista)
        public decimal Amount { get; set; }
        public decimal Interest { get; set; }
        public DateTime DateAdvance { get; set; }

       public Account CreditAccount { get; set; }
       public Account DestinationAccount { get; set; }

    
    }
}
