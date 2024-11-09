using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Infrastructure.Identity.Services;


public class PaymentsController : Controller
{


    private readonly IBankAccountService _bankAccountService;
    private readonly IPaymentService _paymentService;
   private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthenticationResponse _userViewModel;


    public PaymentsController(IBankAccountService bankAccountService, IPaymentService paymentService)
    {
        _bankAccountService = bankAccountService;
        _paymentService = paymentService;
       

    }



    public async Task<IActionResult> Express()
    {

        var model = await _bankAccountService.GetAccounts();
        SavePaymentViewModel vm = new()
        {
            accounts = model

        };
        return View(vm);
    }


    [HttpPost]
    public async Task<IActionResult> Express( SavePaymentViewModel vm)
    {
       
        if (!ModelState.IsValid)
        {
            return RedirectToAction("Express");

        }

        await _paymentService.UpdateAccounts(vm);

        return RedirectToAction("Express");
    }


    public async Task<IActionResult> GetAccountOwner(int accountNumber)
    {


        if (!ModelState.IsValid)
        {
            return RedirectToAction("Express");

        }

        var account = await _bankAccountService.GetUserAccount(accountNumber);
        if (account != null)
        {
            return Json(new { success = true, firstName = account.FirstName, lastName = account.LastName });
        }
        return Json(new { success = false });
    }


}
