using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Advances;
using InternetBanking.Core.Application.ViewModels.Transactions;
using Microsoft.AspNetCore.Mvc;

public class CashAdvanceController : Controller
{

    private readonly IAdvanceService _advanceService;
    private readonly IBankAccountService _bankAccountService;


    public CashAdvanceController (IAdvanceService advanceService, IBankAccountService bankAccountService)
    {
        _advanceService = advanceService;
        _bankAccountService = bankAccountService;
    }



    public async Task<IActionResult> Index()
    {
        var model = await _bankAccountService.GetAccounts();
        SaveAdvanceViewModel vm = new()
        {
            accounts = model
        };

        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> CashAdvance(SaveAdvanceViewModel vm)
    {

        if (!ModelState.IsValid)
        {
            vm.accounts = await _bankAccountService.GetAccounts();
            return View(vm);
        }

        var model = await _advanceService.CashAdvance(vm);

        if (model == null)
        {
            TempData["ErrorMessage"] = "El monto excede el límite de crédito disponible.";
            return RedirectToAction("Index");
        }

        return RedirectToAction("Index");
    }
}
