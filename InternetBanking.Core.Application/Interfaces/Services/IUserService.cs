using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IUserService
    {
     
        Task<AuthenticationResponse> LoginAsync(LoginViewModel vm);
        Task<RegisterResponse> RegisterAsync(SaveUserViewModel vm);
        Task<SaveUserViewModel> ConverToSaveViewModel(AuthenticationResponse vm);
        Task<AuthenticationResponse> UpdateAsync(SaveUserViewModel vm);

        Task<AuthenticationResponse> ValidateUser(string UserName);

        Task SignOutAsync();
    }
}
