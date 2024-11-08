using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using InternetBanking.Core.Application.Helpers;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;


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

        await _paymentService.UpdateAccounts(vm);

        return RedirectToAction("Express");
    }


}
