using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;


namespace WebApp.InternetBanking.Controllers
{
    public class TransactionController : Controller
    {

        private readonly ITransactionService _transactionService;
        private readonly IBankAccountService _bankAccountService;


        public TransactionController(ITransactionService transactionService, IBankAccountService bankAccountService)
        {
            _transactionService = transactionService;
            _bankAccountService = bankAccountService;
        }


        public async Task<IActionResult> Index()
        {
            var model = await _bankAccountService.GetAccounts();
            SaveTransactionViewModel vm = new()
            {
                accounts = model
            };

            return View(vm);

        } 
        
        
        
     

       [HttpPost]
       public async Task<IActionResult> Transaction(SaveTransactionViewModel vm)
        {

            if (!ModelState.IsValid)
            {
                vm.accounts = await _bankAccountService.GetAccounts();
                return RedirectToAction("Index");
            }

            var model = await _transactionService.UpdateAccounts(vm);

            if (model == null)
            {
                TempData["ErrorMessage"] = "No tiene suficiente dinero para realizar esta transacción.";
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }



        

    }
}
