﻿using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Mvc;
using WebApp.InternetBanking.Middlewares;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Enums;
using Microsoft.AspNetCore.Authorization;
using InternetBanking.Core.Application.Services;

namespace WebApp.InternetBanking.Controllers

{ 


    public class UserController : Controller

    {


        private readonly IUserService _userService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;
        private readonly IBankAccountService _bankaccountService;
        private readonly ITransferService _trasnferService;


        public UserController(ITransferService trasnferService, IUserService userService, IBankAccountService bankaccountService, IHttpContextAccessor httpContextAccessor, IAccountService accountService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
            _bankaccountService = bankaccountService;
            _trasnferService = trasnferService;

        }



        [ServiceFilter(typeof(LoginAuthorize))]
        public IActionResult Index()
        {

            return View(new LoginViewModel());
        }

        [ServiceFilter(typeof(LoginAuthorize))]
        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginVm)
        {
            // Verifica si el modelo es válido; si no, retorna la vista con el modelo.
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            // Intenta autenticar al usuario.
            AuthenticationResponse userVm = await _userService.LoginAsync(loginVm);

           
            if (userVm != null && !userVm.HasError)
            {
               
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);


                if (userVm.Roles.Contains(Roles.SuperAdmin.ToString()))
                {
                    return RedirectToRoute(new { Controller = "User", action = "Dashboard" });
                }

                // Verifica si el rol del usuario es 'Client'.
                if (userVm.Roles.Contains(Roles.Client.ToString()))
                {
                    return RedirectToRoute(new { Controller = "BankAccount", action = "Index" });
                }
                // Verifica si el rol del usuario es 'Admin'.
                if (userVm.Roles.Contains(Roles.Admin.ToString()))
                {
                    return RedirectToRoute(new { Controller = "User", action = "Dashboard" });
                }

             

                // Si hay otros roles, puedes manejarlos aquí (opcional).
            }
            else
            {
                // Si hay un error, asigna los detalles del error al modelo de vista.
                loginVm.HasError = userVm.HasError;
                loginVm.Error = userVm.Error;
                return View(loginVm);
            }

            // Si no se ha hecho redirección, devuelve la vista original (opcional).
            return View(loginVm);
        }

        [Authorize(Roles = "Client")]
        public IActionResult Home()
        {
        
            return View();
        }


        //[ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LoadUsers()
        {
            List<AuthenticationResponse> users = await _accountService.GetAllUsersAsync(); 


            return View( "ManageUsers", users);
        }


        //[ServiceFilter(typeof(LoginAuthorize))]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit( string Username)
        {
           var getuser = await _accountService.GetUserByNameAsync(Username); 
           var user = await _userService.ConverToSaveViewModel(getuser); 

    
            return View( "EditUser", user);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> EditUser(SaveUserViewModel vm)
        {
            

            if (vm.Password == null && vm.ConfirmPassword == null)
            {
                ModelState.Remove("Password");
                ModelState.Remove("ConfirmPassword");
            }

            if (!ModelState.IsValid)
            {

                return View("EditUser", vm);
            }

         
           var updateResponse = await _userService.UpdateAsync(vm);

            var registerammount = await _bankaccountService.AddAmountUser(vm.InitialAmount ?? 0, vm.Id);

            if (registerammount == null)
            {
                TempData["ErrorMessage"] = "El usuario al que esta intentado añadir el monto no cuenta con una cuenta de ahorro principal";
                return View("EditUser", vm);
            }

            return RedirectToAction("LoadUsers");
        }


        [Authorize(Roles = "Admin")]
        public IActionResult Create()
        {
            return View("EditUser", new SaveUserViewModel () );
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateUser( SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("EditUser", vm);
            }
            
            var user = await _userService.RegisterAsync(vm);

            return RedirectToAction("LoadUsers");
        }


        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> StateUser(string UserName)
        {
             await _userService.ValidateUser(UserName);


            return RedirectToAction("LoadUsers");
        }


       
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Dashboard()
        {
            var numberproducts = await _trasnferService.GetSystemReport(); 
            return View("Dashboard", numberproducts);
        }


        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

        [Authorize]
        public  IActionResult AccessDenied()
        {
           
            return View();
        }

    }
}
