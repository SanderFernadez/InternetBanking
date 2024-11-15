using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Infrastructure.Identity.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;


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
            response.Cedula = user.Cedula;


            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            response.Roles = rolesList.ToList();

            response.IsVerified = user.EmailConfirmed;


            return response;
        }



        public async Task<List<AuthenticationResponse>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();
            var responseList = new List<AuthenticationResponse>();

            foreach (var user in users)
            {
                var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

                var response = new AuthenticationResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Cedula = user.Cedula,
                    PhoneNumber = user.PhoneNumber,
                    Email = user.Email,
                    IsVerified = user.EmailConfirmed,
                    Roles = rolesList.Count > 2 ? new List<string> { Roles.SuperAdmin.ToString() } : rolesList.ToList(),
                    HasError = false
                };

                responseList.Add(response);
            }

            return responseList;
        }



        public async Task<AuthenticationResponse> GetUserByNameAsync(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return null;
            }

            var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

            AuthenticationResponse response = new()
            {
                Id = user.Id,
                UserName = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
                Email = user.Email,
                Cedula = user.Cedula,
                //Password = user.PasswordHash,
                IsVerified = user.EmailConfirmed,
                Roles = rolesList.Count > 2 ? new List<string> { Roles.SuperAdmin.ToString() } : rolesList.ToList(),
                HasError = false,
            };

            return response;
        }

        public async Task<RegisterResponse> RegisterBasicAsync(RegisterRequest request)
        {
            RegisterResponse response = new()
            {
                HasError = false
            };

            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                response.HasError = true;
                response.Error = $"Username {request.UserName} is already taken.";
                return response;
            }

            var userWithSameUserEmail = await _userManager.FindByEmailAsync(request.Email);
            if (userWithSameUserEmail != null)
            {
                response.HasError = true;
                response.Error = $"Email {request.Email} is already registered.";
                return response;
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                PhoneNumber = request.PhoneNumber,
                Cedula = request.Cedula
            };

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                response.HasError = true;
                response.Error = "An error occurred trying to register the user.";
                return response;
            }

            // Asignar rol por defecto
            var roles = request.Roles?.Any() == true ? request.Roles : new List<string> { "Client" };
            foreach (var role in roles)
            {
                var roleResult = await _userManager.AddToRoleAsync(user, role);
                if (!roleResult.Succeeded)
                {
                    response.HasError = true;
                    response.Error = "An error occurred while assigning the role.";
                    return response;
                }
            }

            // Retornar la respuesta final al finalizar el registro exitoso
            return response; // Asegúrate de que este retorno esté aquí
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

            // Actualización de propiedades del usuario
            user.UserName = vm.UserName;
            user.FirstName = vm.FirstName;
            user.LastName = vm.LastName;
            user.Email = vm.Email;
            user.EmailConfirmed = vm.IsVerified;
            user.PhoneNumber = vm.PhoneNumber;
            user.Cedula = vm.Cedula;

            // Actualizar la contraseña si se proporcionó una nueva
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

            // Actualizar el usuario
            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                return null;
            }

            // Gestionar roles: eliminamos todos los roles actuales y luego los añadimos desde la lista en vm
            var currentRoles = await _userManager.GetRolesAsync(user);
            await _userManager.RemoveFromRolesAsync(user, currentRoles);

            if (vm.Roles != null)
            {
                foreach (var role in vm.Roles)
                {
                    await _userManager.AddToRoleAsync(user, role);

                }
            }

            // Actualizar el modelo de respuesta con los datos actualizados
            var roll = (await _userManager.GetRolesAsync(user)).ToList();
            vm.UserName = user.UserName;
            vm.FirstName = user.FirstName;
            vm.LastName = user.LastName;
            vm.Email = user.Email;
            vm.Cedula = user.Cedula;
            vm.PhoneNumber = user.PhoneNumber;
            vm.Roles = roll;

            return vm;
        }

    }
}

