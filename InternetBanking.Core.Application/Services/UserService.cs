using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Services
{
    public class UserService : IUserService
    {

        private readonly IAccountService _accountService;

        private readonly IMapper _mapper;


        public UserService(IMapper mapper, IAccountService accountService)

        {
            _accountService = accountService;
            _mapper = mapper;


        }

      
      public async Task<AuthenticationResponse> LoginAsync(LoginViewModel vm)
        {
            AuthenticationRequest loginRequest = _mapper.Map<AuthenticationRequest>(vm);
            AuthenticationResponse userResponse = await _accountService.AuthenticateAsync(loginRequest);
            return userResponse;
        }

        public async Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm, string origin)
        {
            RegisterRequest registerRequest = _mapper.Map<RegisterRequest>(vm);
            return await _accountService.RegisterBasicAsync(registerRequest, origin);
        }

        public async  Task<SaveUserViewModel> ConverToSaveVewModel(AuthenticationResponse vm)
        {
            SaveUserViewModel user = new SaveUserViewModel
            {
                Id = vm.Id,
                UserName = vm.UserName,
                FirstName = vm.FirstName,
                LastName = vm.LastName,
                Email = vm.Email,
                PhoneNumber = vm.PhoneNumber,
                
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

        public async Task SignOutAsync()
        {
            await _accountService.SignOutAsync();
        }
    }






   
 
   
}

