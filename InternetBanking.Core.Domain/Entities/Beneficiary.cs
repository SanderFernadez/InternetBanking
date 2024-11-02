

namespace InternetBanking.Core.Domain.Entities
{
    public class Beneficiary
    {
        public int Id { get; set; }

        public string UserId { get; set; } 
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public int BeneficiaryAccount { get; set; } 
    }    
}
