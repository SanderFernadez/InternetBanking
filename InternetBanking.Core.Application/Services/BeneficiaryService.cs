using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace InternetBanking.Core.Application.Services
{
       public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
        {
            private readonly IBeneficiaryRepository _beneficiaryRepository;
            private readonly IBankAccountService _bankAccountService;
            private readonly IMapper _mapper;
            private readonly IHttpContextAccessor _httpContextAccessor;
            private readonly AuthenticationResponse _userViewModel;
            private readonly IAccountService _accountService;

        public BeneficiaryService(IAccountService accountService ,IBankAccountService bankAccountService,IBeneficiaryRepository beneficiaryRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor) : base(beneficiaryRepository, mapper)
            {
                 _beneficiaryRepository = beneficiaryRepository;
                 _mapper = mapper;
                _httpContextAccessor = httpContextAccessor;
                _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");
                _bankAccountService = bankAccountService;
                _accountService = accountService;
        }



        public async Task<List<BeneficiaryViewModel>> LoadBeneficiary()
        {
            // Llama al método para obtener todos los beneficiarios.
            var beneficiaries = await GetAllViewModel();

            // Filtra la lista para que solo incluya beneficiarios del usuario actual y proyecta cada elemento.
            var list = beneficiaries
                .Where(b => b.UserId == _userViewModel.Id)
                .Select(b => new BeneficiaryViewModel
                {
                    Id = b.Id,
                    FirstName = b.FirstName,
                    LastName = b.LastName,
                    BeneficiaryAccount = b.BeneficiaryAccount,
                    UserId = b.UserId,
                    // Agrega aquí otros campos necesarios de BeneficiaryViewModel.
                })
                .ToList();

            return list;
        }





   




        public async Task<BeneficiaryViewModel?> AddBeneficiaryAccount(int beneficiaryAccount)
        {
            // Obtener todas las cuentas bancarias, usuarios y beneficiarios actuales
            var accounts = await _bankAccountService.GetAllViewModel();
            var users = await _accountService.GetAllUsersAsync();
            var beneficiaries = await GetAllViewModel();

            // Verificar si el número de cuenta proporcionado existe en las cuentas bancarias
            var account = accounts.FirstOrDefault(a => a.AccountNumber == beneficiaryAccount);

            // Retornar null si la cuenta no existe
            if (account == null)
            {
                return null;
            }

            // Verificar si el beneficiario ya existe en la lista de beneficiarios
            var existingBeneficiary = beneficiaries.FirstOrDefault(b => b.BeneficiaryAccount == beneficiaryAccount  && b.UserId == _userViewModel.Id);
            if (existingBeneficiary != null)
            {
                return null;
            }

            // Obtener el usuario asociado a la cuenta
            var user = users.FirstOrDefault(u => u.Id == account.UserId);

            // Retornar null si el usuario no existe
            if (user == null)
            {
                return null;
            }

            // Crear el SaveBeneficiaryViewModel con los datos necesarios
            var beneficiary = new SaveBeneficiaryViewModel
            {
                BeneficiaryAccount = account.AccountNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = _userViewModel.Id
            };

            // Guardar el beneficiario en la base de datos
            await base.Add(beneficiary);

            // Retornar el BeneficiaryViewModel con los datos
            var beneficiaryViewModel = new BeneficiaryViewModel
            {
                BeneficiaryAccount = account.AccountNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserId = _userViewModel.Id
            };

            return beneficiaryViewModel;
        }







    }

}
