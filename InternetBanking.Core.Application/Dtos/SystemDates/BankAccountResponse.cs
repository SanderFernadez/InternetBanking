
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;

namespace InternetBanking.Core.Application.Dtos.SystemDates
{
    public class BankAccountResponse
    {
        //public ICollection<AuthenticationResponse>? TotalUsers { get; set; }
        //public ICollection<BankAccountViewModel>? TotalAccounts { get; set; }
        //public ICollection<PaymentViewModel>? TotalPayments { get; set; }
        //public ICollection<TransactionViewModel>? TotalTransactions { get; set; } 
        
        
        public int? TotalUsers { get; set; }
        public int? TotalAccounts { get; set; }
        public int? TotalPayments { get; set; }
        public int? TotalTransactions { get; set; }
        public int? DailyTransactionsCount { get; set; }
        public int? DailyPaymentsCount { get; set; }
        public int? InactiveUsersCount { get; set; }
    }
}
