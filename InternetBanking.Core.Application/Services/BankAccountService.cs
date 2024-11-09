using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;

using InternetBanking.Core.Domain.Entities;
using InternetBanking.Core.Domain.Enums;
using Microsoft.AspNetCore.Http;


namespace InternetBanking.Core.Application.Services
{
    public class BankAccountService : GenericService<SaveBankAccountViewModel, BankAccountViewModel, Account>, IBankAccountService
    {
        private readonly IBankAccountRepository _bankAccountRepositor;
        private readonly IAccountService _accountService;
        //private readonly IPaymentService _paymentService;
        private readonly ITransactionService _transactionService;
        private readonly IMapper _mapper;
       
        private readonly AuthenticationResponse _userViewModel;
        private static readonly Random _random = new Random();
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BankAccountService(IBankAccountRepository bankAccountRepository, /*IPaymentService paymentService, */ IAccountService accountService , IMapper mapper, IHttpContextAccessor httpContextAccessor, ITransactionService transactionService) : base(bankAccountRepository, mapper)
        {
            _bankAccountRepositor = bankAccountRepository;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
           // _paymentService = paymentService;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
            _transactionService = transactionService;
        }




        public override async Task<SaveBankAccountViewModel> Add(SaveBankAccountViewModel vm)
        {
            
           return await base.Add(vm);
        }


        public override async Task Update(SaveBankAccountViewModel vm, int Id)
        {
           
            await base.Update(vm, Id);
        }

        public async Task<int> NumberOfProductsClient()
        {
            var clients = await GetAllViewModel(); // Espera a que se complete la operación

            // Inicializa un contador para los productos
            int totalProducts = clients.Count; // Obtiene el número de clientes (o productos según tu lógica)

            return totalProducts;
        }










        public async Task<AuthenticationResponse> GetUserAccount(int accountnumber)
        {
            // Llama al método para obtener todos los beneficiarios.
            var accounts = await GetAllViewModel();
            var users = await _accountService.GetAllUsersAsync();

            var selectedAccount = accounts.FirstOrDefault(a => a.AccountNumber == accountnumber);

            if (selectedAccount == null)
            {
                // Devuelve una lista vacía si no se encuentra la cuenta
                return null;
            }


            var selectedUser =users.FirstOrDefault(u => u.Id == selectedAccount.UserId);


            if (selectedUser == null)
            {
                // Devuelve una lista vacía si no se encuentra la cuenta
                return null;
            }

            var response = new AuthenticationResponse
            { 
                Id = selectedUser.Id,
                FirstName = selectedUser.FirstName,
                LastName = selectedUser.LastName,
                Email = selectedUser.Email,
            
            
            };



            return response;
        }









        public async Task<List<BankAccountViewModel>> GetAccounts()
        {
            var accounts = await GetAllViewModel();
            var accountsuser = accounts
                 .Where(p => p.UserId == _userViewModel.Id).ToList();

            return  accountsuser;
        }





        public async Task<List<BankAccountViewModel>> GetDatesOfSystem()
        {
            var products = await GetAllViewModel();
            var users = await _accountService.GetAllUsersAsync();
            //var payments = await _paymentService.GetAllViewModel();
            var transactions = await _transactionService.GetAllViewModel();

            foreach (var activeUsers in products)
            {
                activeUsers.Users = users; 
                //activeUsers.Payments = payments; 
                activeUsers.Transactions = transactions; 
            }

            return products;
        }


        public int GenerateAccountNumber()
        {
            return _random.Next(100000000, 1000000000); 
        }

        public async Task<List<BankAccountViewModel>> GetClientProducts(string UserId)
        {
            var products = await GetAllViewModel();
            var userproducts = products
                .Where(p => p.UserId == UserId).ToList();

            return userproducts; 
        }

        public async Task CreateProduct(AccountType accountType, string UserId, decimal creditLimit, decimal loanAmount)
        {
            // Inicializa el ViewModel con los valores necesarios
            SaveBankAccountViewModel vm = new SaveBankAccountViewModel
            {
                AccountType = accountType,
                AccountNumber = GenerateAccountNumber(),
                InitialAmount = 0,
                CurrentBalance = 0,
                UserId = UserId,
                CreditLimit = accountType == AccountType.Credit ? creditLimit : 0,
                LoanAmount = accountType == AccountType.Loan ? loanAmount : 0,
            };

            // Si es una cuenta de tipo 'Loan', se agrega el monto del préstamo al balance
            if (accountType == AccountType.Loan)
            {
                // Actualiza el balance en la cuenta principal
                var updatedBalance = await AddLoan(loanAmount, UserId);
                
            }

            // Agrega el producto al sistema
            await base.Add(vm);
        }


        public async Task<decimal> AddLoan(decimal loan, string UserId)
        {
            // Obtiene las cuentas que pertenecen al UserId y filtra la cuenta principal
            var accounts = await GetAllViewModel();
            var selectedAccount = accounts.FirstOrDefault(a => a.UserId == UserId && a.AccountType == AccountType.SavingPrincipal);

            // Verifica si se encontró la cuenta principal
            if (selectedAccount != null)
            {
                // Suma el monto del préstamo al balance actual de la cuenta principal
                selectedAccount.CurrentBalance += loan;

                // Mapea la cuenta seleccionada a un SaveBankAccountViewModel para la actualización
                SaveBankAccountViewModel vm = new SaveBankAccountViewModel
                {
                    Id = selectedAccount.Id,
                    AccountType = selectedAccount.AccountType,
                    AccountNumber = selectedAccount.AccountNumber,
                    InitialAmount = selectedAccount.InitialAmount,
                    CurrentBalance = selectedAccount.CurrentBalance,
                    UserId = selectedAccount.UserId,
                    CreditLimit = selectedAccount.AccountType == AccountType.Credit ? selectedAccount.CreditLimit : 0,
                    LoanAmount = selectedAccount.LoanAmount + loan, // Actualiza el monto del préstamo acumulado
                };

                // Actualiza la cuenta con el nuevo balance
                await base.Update(vm, vm.Id);

                // Retorna el balance actualizado
                return vm.CurrentBalance;
            }
            else
            {
                throw new Exception("Cuenta principal no encontrada para el UserId proporcionado");
            }
        }





    }
}
