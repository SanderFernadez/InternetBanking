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




        //[ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> LoadUsers()
        {
            List<AuthenticationResponse> users = await _accountService.GetAllUsersAsync(); 


            return View( "ManageUsers", users);
        }
        
        
        //[ServiceFilter(typeof(LoginAuthorize))]
        public async Task<IActionResult> Edit( string Username)
        {
           var getuser = await _accountService.GetUserByNameAsync(Username); 
           var user = await _userService.ConverToSaveViewModel(getuser); 

    
            return View( "EditUser", user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(SaveUserViewModel vm)
        {
            //ModelState.Remove("Roles");
            ModelState.Remove("ConfirmPassword");
            if (!ModelState.IsValid)
            {

                return View("EditUser", vm);
            }

         //  var userexist = await _accountService.GetUserByNameAsync(vm.UserName);
           var updateResponse = await _userService.UpdateAsync(vm);

            return RedirectToAction("LoadUsers");
        }



        public async Task<IActionResult> Create()
        {
           

            return View("EditUser", new SaveUserViewModel () );
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser( SaveUserViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                return View("EditUser", vm);
            }
            
            var user = await _userService.RegisterAsync(vm);

            return RedirectToAction("LoadUsers");
        }



           public async Task<IActionResult> StateUser(string UserName)
        {
             await _userService.ValidateUser(UserName);


            return RedirectToAction("LoadUsers");
        }

       




        public async Task<IActionResult> LogOut()
        {
            await _userService.SignOutAsync();
            HttpContext.Session.Remove("user");
            return RedirectToRoute(new { controller = "User", action = "Index" });
        }

    }
}
