using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Transfers
{
    public class TransferViewModel
    {
        public int Id { get; set; } 
        public int AccountSourceId { get; set; } 
      //  public string SourceAccountName { get; set; } // Nombre de la cuenta de origen (opcional, para mostrar en la vista)
        public int DestinationAccountId { get; set; } 
        public decimal Amount { get; set; } 
        public DateTime TransferDate { get; set; } 
    }
}
