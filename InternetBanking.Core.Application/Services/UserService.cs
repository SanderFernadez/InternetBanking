﻿using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Users;
using InternetBanking.Core.Domain.Enums;


namespace InternetBanking.Core.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IAccountService _accountService;
        private readonly IBankAccountService _bankAccountService;

        private readonly IMapper _mapper;


        public UserService(IMapper mapper, IBankAccountService bankAccountService, IAccountService accountService)

        {
            

            _accountService = accountService;
            _mapper = mapper;
            _bankAccountService = bankAccountService;


        }

      
      public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm)

        {
            

            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            var register = await _accountService.RegisterBasicAsync(registerRequest);


            var user = await _accountService.GetUserByNameAsync(vm.UserName);
            SaveBankAccountViewModel account = new SaveBankAccountViewModel
            {
                AccountType = AccountType.SavingPrincipal,
                InitialAmount = vm.InitialAmount ?? 0,
                UserId = user.Id, // Puedes establecer el valor adecuado una vez que el usuario sea registrado
                CurrentBalance = vm.InitialAmount ?? 0,
                CreditLimit = 0,
                LoanAmount = 0,
                AccountNumber = _bankAccountService.GenerateAccountNumber()
            };

            await _bankAccountService.Add(account);



            return register;


        }



        public async  Task<SaveUserViewModel> ConverToSaveViewModel(AuthenticationResponse vm)
        {
            SaveUserViewModel user = new()
            {
                Id = vm.Id,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                IsVerified = vm.IsVerified,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                Cedula = vm.Cedula,
                Roles = vm.Roles,
                
            };

            return user;
        }

        public async  Task<AuthenticationResponse> UpdateAsync(SaveUserViewModel vm)

        {

            AuthenticationResponse UpdateRequest = _mapper.Map<AuthenticationResponse>(vm);

            var updateResponse = await _accountService.UpdateUser(UpdateRequest);

            if (updateResponse == null)
            {
                throw new Exception("No se pudo actualizar el usuario. El usuario no existe.");
            }

            return updateResponse;
        }















        public async Task<AuthenticationResponse> ValidateUser(string userName)
        {
            // Validar el nombre de usuario antes de continuar
            if (string.IsNullOrWhiteSpace(userName))
            {
                throw new ArgumentException("El nombre de usuario no puede ser nulo o vacío.", nameof(userName));
            }

            // Verificar si el usuario existe
            var existingUser = await _accountService.GetUserByNameAsync(userName);
            if (existingUser == null)
            {
                throw new KeyNotFoundException($"El usuario con el nombre {userName} no existe.");
            }

            // Aquí puedes agregar la lógica para actualizar la propiedad deseada.
            // Por ejemplo, supongamos que solo deseas actualizar el estado del usuario.
            existingUser.IsVerified = !existingUser.IsVerified; // Cambiar el estado activo/inactivo.

            // Realizar la actualización
            var updateResponse = await _accountService.UpdateUser(existingUser);
            if (updateResponse == null)
            {
                throw new Exception("No se pudo actualizar el usuario debido a un error interno.");
            }

            return updateResponse;
        }



        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }
    }






   
 
   
}

