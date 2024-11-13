

namespace InternetBanking.Core.Application.Dtos.SystemDates
{
    public class BankAccountResponse
    {
        
        public int? TotalUsers { get; set; }
        public int? TotalAccounts { get; set; }
        public int? TotalPayments { get; set; }
        public int? TotalTransactions { get; set; }
        public int? DailyTransactionsCount { get; set; }
        public int? DailyPaymentsCount { get; set; }
        public int? InactiveUsersCount { get; set; }
    }
}
