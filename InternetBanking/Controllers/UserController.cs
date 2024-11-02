using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApp.InternetBanking.Middlewares;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.Interfaces.Services;

namespace WebApp.InternetBanking.Controllers

{ 


    public class UserController : Controller

    {


        private readonly IUserService _userService;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IAccountService _accountService;


        public UserController(IUserService userService, IHttpContextAccessor httpContextAccessor, IAccountService accountService)
        {
            _userService = userService;
            _httpContextAccessor = httpContextAccessor;
            _accountService = accountService;
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
            if (!ModelState.IsValid)
            {
                return View(loginVm);
            }

            AuthenticationResponse userVm = await _userService.LoginAsync(loginVm);

            if (userVm != null && userVm.HasError != true)
            {
                HttpContext.Session.Set<AuthenticationResponse>("user", userVm);
                return RedirectToRoute(new { Controller = "Home", action = "Index" });
            }

            else
            {
                loginVm.HasError = userVm.HasError;
                loginVm.Error = userVm.Error;
                return View(loginVm);
            }


        }


        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

    }
}
