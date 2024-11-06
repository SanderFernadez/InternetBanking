
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
        private readonly IAccountService _accountService;
        private readonly IPaymentService _paymentService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
       
        private readonly AuthenticationResponse _userViewModel;
        private static readonly Random _random = new Random();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankAccountService(IBankAccountRepository bankAccountRepository, IPaymentService paymentService, IAccountService accountService , IMapper mapper, IHttpContextAccessor httpContextAccessor, ITransactionService transactionService) : base(bankAccountRepository, mapper)
        {
            _bankAccountRepositor = bankAccountRepository;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _paymentService = paymentService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _transactionService = transactionService;
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

        public async Task<int> NumberOfProductsClient()
        {
            var clients = await GetAllViewModel(); // Espera a que se complete la operación

            // Inicializa un contador para los productos
            int totalProducts = clients.Count; // Obtiene el número de clientes (o productos según tu lógica)

            return totalProducts;
        }

        public async Task<List<BankAccountViewModel>> GetDatesOfSystem()
        {
            var products = await GetAllViewModel();
            var users = await _accountService.GetAllUsersAsync();
            var payments = await _paymentService.GetAllViewModel();
            var transactions = await _transactionService.GetAllViewModel();

            foreach (var activeUsers in products)
            {
                activeUsers.Users = users; 
                activeUsers.Payments = payments; 
                activeUsers.Transactions = transactions; 
            }

            return products;
        }


        public int GenerateAccountNumber()
        {
            return _random.Next(100000000, 1000000000); 
        }

        
       


    }
}
