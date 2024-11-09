
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;

namespace InternetBanking.Core.Application.Dtos.SystemDates
{
    public class BankAccountResponse
    {
        public ICollection<AuthenticationResponse>? Users { get; set; }
        public ICollection<BankAccountViewModel>? accounts { get; set; }
        public ICollection<PaymentViewModel>? Payments { get; set; }
        public ICollection<TransactionViewModel>? Transactions { get; set; }
    }
}
