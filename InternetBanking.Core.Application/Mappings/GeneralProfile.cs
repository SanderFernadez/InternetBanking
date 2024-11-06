using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Advances;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.ViewModels.Transactions;
using InternetBanking.Core.Application.ViewModels.Transfers;
using InternetBanking.Core.Application.ViewModels.Users;
using InternetBanking.Core.Domain.Entities;


namespace InternetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile

    { 
        public GeneralProfile()
    {

            #region BankAccount

            CreateMap<Account, SaveBankAccountViewModel>()
              .ReverseMap()
              .ForMember(x => x.Transactions, opt => opt.Ignore())
              .ForMember(x => x.Advances, opt => opt.Ignore())
              .ForMember(x => x.OriginTransfers, opt => opt.Ignore())
              .ForMember(x => x.DestinationTransfers, opt => opt.Ignore());

            CreateMap<Account, BankAccountViewModel>()
               .ReverseMap()
              .ForMember(x => x.Transactions, opt => opt.Ignore())
              .ForMember(x => x.Advances, opt => opt.Ignore())
              .ForMember(x => x.OriginTransfers, opt => opt.Ignore())
              .ForMember(x => x.DestinationTransfers, opt => opt.Ignore());

            CreateMap<BankAccountViewModel, SaveBankAccountViewModel>();

            #endregion

            #region Payment

            CreateMap<Payment, SavePaymentViewModel>()
            .ReverseMap()
            .ForMember(x => x.Transaction,opt => opt.Ignore());

            CreateMap<Payment, PaymentViewModel>()
             .ReverseMap()
            .ForMember(x => x.Transaction, opt => opt.Ignore());

            CreateMap<PaymentViewModel, SavePaymentViewModel>();



            #endregion


            #region Benificiary
            CreateMap<Beneficiary, SaveBeneficiaryViewModel>()
                .ReverseMap();

            CreateMap<Beneficiary, BeneficiaryViewModel>();

            CreateMap<BeneficiaryViewModel, SaveBeneficiaryViewModel>();

            #endregion

            #region Tranfer

           
            CreateMap<Transfer, SaveTransferViewModel>()
                .ReverseMap()
                .ForMember(x => x.SourceAccount, opt => opt.Ignore())
                .ForMember(x => x.DestinationAccount, opt => opt.Ignore());

            CreateMap<Transfer, TransferViewModel>()
                .ReverseMap()
                .ForMember(x => x.SourceAccount, opt => opt.Ignore())
                .ForMember(x => x.DestinationAccount, opt => opt.Ignore())
                ;
            CreateMap<TransferViewModel, SaveTransferViewModel>();
            #endregion


            #region Transaction

            CreateMap<Transaction, SaveTransactionViewModel>()
              .ReverseMap()
              .ForMember(x => x.Account, opt => opt.Ignore())
              .ForMember(x => x.Payment, opt => opt.Ignore());

            CreateMap<Transaction, TransactionViewModel>()
                .ReverseMap()
                .ForMember(x => x.Account, opt => opt.Ignore())
              .ForMember(x => x.Payment, opt => opt.Ignore());
   
            CreateMap<TransactionViewModel, SaveTransactionViewModel>();
            #endregion


            #region Advance

            CreateMap<Advance, SaveAdvanceViewModel>()
              .ReverseMap()
              .ForMember(x => x.CreditAccount, opt => opt.Ignore())
              .ForMember(x => x.DestinationAccount, opt => opt.Ignore());

            CreateMap<Advance, AdvanceViewModel>()
              .ReverseMap()
              .ForMember(x => x.CreditAccount, opt => opt.Ignore())
              .ForMember(x => x.DestinationAccount, opt => opt.Ignore());

            CreateMap<AdvanceViewModel, SaveAdvanceViewModel>();
            #endregion

            #region user

            CreateMap<SaveUserViewModel, AuthenticationResponse>()

                .ReverseMap()               
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore());

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<AuthenticationResponse, RegisterRequest>()
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.IsVerified, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());



            CreateMap<RegisterRequest, SaveUserViewModel>()
               .ForMember(x => x.HasError, opt => opt.Ignore())
               .ForMember(x => x.Error, opt => opt.Ignore())
               .ReverseMap();

            #endregion
        }

    }
}
