
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Domain.Enums;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.InternetBanking.Controllers
{
    public class BankAccountController : Controller
    {
        private readonly IBankAccountService _bankAccountService;

        public BankAccountController(IBankAccountService bankAccountService)
        {
            _bankAccountService = bankAccountService;
        }

        public IActionResult Index()
        {
            return View();
        }
        

        public async Task<IActionResult> EditProduct(string UserId)
        {
            var products = await _bankAccountService.GetClientProducts(UserId);
            ViewData["UserId"] = UserId; // Almacena UserId en ViewData
            return View("Products",products);
        }


        [HttpPost]
        public async Task<IActionResult> CreateProduct(string UserId, AccountType accountType, decimal creditLimit, decimal loanAmount)
        {
            await _bankAccountService.CreateProduct(accountType, UserId, creditLimit, loanAmount);
            return RedirectToAction("EditProduct", new { UserId = UserId }); // Redirige con el parámetro UserId
        } 
        
        [HttpPost]
        public async Task<IActionResult> DeleteProduct(int Id, string UserId)
        {
            await _bankAccountService.Delete(Id);
            return RedirectToAction("EditProduct", new { UserId = UserId }); // Redirige con el parámetro UserId
        }




    }
}
