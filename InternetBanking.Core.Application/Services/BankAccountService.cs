

using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InternetBanking.Core.Application.Services
{
    public class BankAccountService : GenericService<SaveBankAccountViewModel, BankAccountViewModel, Account>, IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepositor;
        private readonly IMapper _mapper;
       
        private readonly AuthenticationResponse _userViewModel;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(bankAccountRepository, mapper)
        {
            _bankAccountRepositor = bankAccountRepository;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
        }




        public override async Task<SaveBankAccountViewModel> Add(SaveBankAccountViewModel vm)
        {
            //vm.UserId = _userViewModel.Id;
           //vm.CreatedAt = DateTime.Now;
           return await base.Add(vm);
        }


        public override async Task Update(SaveBankAccountViewModel vm, int Id)
        {
           // vm.UserId = _userViewModel.Id;
            //vm.CreatedAt = DateTime.Now;
           // await base.Update(vm, Id);
        }
















    }
}
