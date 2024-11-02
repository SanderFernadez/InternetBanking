using InternetBanking.Core.Application.Dtos.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Interfaces.Services
{
     public interface IAccountService
    {
        Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request);
        
        Task<RegisterResponse> RegisterBasicAsync(RegisterRequest request, string origin);

        Task<AuthenticationResponse> UpdateUser(AuthenticationResponse vm);
        Task<AuthenticationResponse> GetUserByNameAsync(string userName);

        Task<List<AuthenticationResponse>> GetAllUsersAsync();

        Task SignOutAsync();
    }
}
