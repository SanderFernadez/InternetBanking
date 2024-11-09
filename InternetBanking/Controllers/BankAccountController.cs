
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Domain.Enums;
using InternetBanking.Core.Application.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace WebApp.InternetBanking.Controllers
{
    [Authorize]
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _bankAccountService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly AuthenticationResponse _userViewModel;

        public BankAccountController(IBankAccountService bankAccountService, IHttpContextAccessor httpContextAccessor)
        {
            _bankAccountService = bankAccountService;
            _httpContextAccessor = httpContextAccessor;
            _userViewModel = _httpContextAccessor.HttpContext.Session.Get<AuthenticationResponse>("user");

        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Index()
        {
            var products = await _bankAccountService.GetClientProducts(_userViewModel.Id);
            ViewData["UserId"] = _userViewModel.Id; // Almacena UserId en ViewData
            return View(products);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> EditProduct(string UserId)
        {
            var products = await _bankAccountService.GetClientProducts(UserId);
            ViewData["UserId"] = UserId; // Almacena UserId en ViewData
            return View("Products",products);
        }


        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> CreateProduct(string UserId, AccountType accountType, decimal creditLimit, decimal loanAmount)
        {
            await _bankAccountService.CreateProduct(accountType, UserId, creditLimit, loanAmount);
            return RedirectToAction("EditProduct", new { UserId = UserId }); // Redirige con el parámetro UserId
        } 
        
        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> DeleteProduct(int Id, string UserId)
        {
            await _bankAccountService.Delete(Id);
            return RedirectToAction("EditProduct", new { UserId = UserId }); // Redirige con el parámetro UserId
        }




    }
}
