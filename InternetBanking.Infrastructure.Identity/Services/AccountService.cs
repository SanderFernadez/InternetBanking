using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Identity.Services
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
       

        public AccountService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            

        }

        public async Task<AuthenticationResponse> AuthenticateAsync(AuthenticationRequest request)
        {
            AuthenticationResponse response = new();

            var user = await _userManager.FindByNameAsync(request.UserName);

            if (user == null)
            {
                response.HasError = true;
                response.Error = $"No Account registered with {request.UserName}";
                return response;
            }

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

            if (!result.Succeeded)
            {

                response.HasError = true;
                response.Error = $"Invalid Credentials for {request.UserName}";
                return response;
            }

            if (!user.EmailConfirmed)
            {

                response.HasError = true;
                response.Error = $"Account no Confirmed for {request.UserName}";
                return response;
            }

            response.Id = user.Id;
            response.Email = user.Email;
            response.UserName = user.UserName;
            response.FirstName = user.FirstName;
            response.LastName = user.LastName;
            response.PhoneNumber = user.PhoneNumber;


            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();

            response.IsVerified = user.EmailConfirmed;


            return response;
        }

    

        public async Task<List<AuthenticationResponse>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();


            var responseList = users.Select(user => new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                IsVerified = user.EmailConfirmed,
                HasError = false
            }).ToList();

            return responseList;
        }

        public async Task<AuthenticationResponse> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }


            AuthenticationResponse response = new AuthenticationResponse
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                IsVerified = user.EmailConfirmed,
                HasError = false,
            };

            return response;
        }

        public async Task<RegisterResponse> RegisterBasicAsync(RegisterRequest request, string origin)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);

            if (userWithSameUserName != null)
            {

                response.HasError = true;
                response.Error = $" Username {request.UserName} is already taken.";
                return response;

            }

            var userWithSameUserEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameUserEmail != null)
            {

                response.HasError = true;
                response.Error = $" Email {request.Email} is Already Register.";
                return response;

            }

            var user = new ApplicationUser

            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
               
            };

            var result = await _userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {

                response.HasError = true;
                response.Error = $" An Error Ocurred Trying to Register the User.";
                return response;

            }


            return response;
        }

        public async Task SignOutAsync()
        {
            await _signInManager.SignOutAsync();
        }

        public async Task<AuthenticationResponse> UpdateUser(AuthenticationResponse vm)
        {


            var user = await _userManager.FindByIdAsync(vm.Id);

            if (user == null)
            {

                return null;
            }


            user.UserName = vm.UserName;
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Email = vm.Email;
            user.PhoneNumber = vm.PhoneNumber;
            


            if (!string.IsNullOrEmpty(vm.Password))
            {

                var removePasswordResult = await _userManager.RemovePasswordAsync(user);
                if (!removePasswordResult.Succeeded)
                {

                    return null;
                }


                var addPasswordResult = await _userManager.AddPasswordAsync(user, vm.Password);
                if (!addPasswordResult.Succeeded)
                {

                    return null;
                }
            }


            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {

                vm.UserName = user.UserName;
                vm.FirstName = user.FirstName;
                vm.LastName = user.LastName;
                vm.Email = user.Email;
                vm.PhoneNumber = user.PhoneNumber;
               

                return vm;
            }


            return null;
        }
    }
}
