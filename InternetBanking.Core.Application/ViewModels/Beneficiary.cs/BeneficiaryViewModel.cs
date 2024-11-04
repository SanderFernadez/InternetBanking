using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.ViewModels.Beneficiary.cs
{
    public class BeneficiaryViewModel
    {
        public int Id { get; set; } 
        public string UserId { get; set; } 
        public string FirstName { get; set; }
        public string LastName { get; set; } 
        public int BeneficiaryAccount { get; set; } 

    }
}
