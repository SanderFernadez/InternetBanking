using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;

[Authorize(Roles = "Client")]
public class PaymentsController : Controller
{


    private readonly IBankAccountService _bankAccountService;
    private readonly IPaymentService _paymentService;
    private readonly IBeneficiaryService _beneficiariesService;
   private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly AuthenticationResponse _userViewModel;


    public PaymentsController(IBeneficiaryService beneficiariesService, IBankAccountService bankAccountService, IPaymentService paymentService)
    {
        _bankAccountService = bankAccountService;
        _paymentService = paymentService;
        _beneficiariesService = beneficiariesService;
       

    }


    // Método GET para cargar la vista inicial con las cuentas
    public async Task<IActionResult> Express()
    {
        var model = await _bankAccountService.GetAccounts();
        SavePaymentViewModel vm = new()
        {
            accounts = model
        };

        return View(vm);
    }

    // Método POST para procesar el formulario de pago
    [HttpPost]
    public async Task<IActionResult> Express(SavePaymentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            // Enviar nuevamente la lista de cuentas al modelo en caso de errores de validación
            vm.accounts = await _bankAccountService.GetAccounts();
            return View(vm);  // Muestra la misma vista con errores
        }

        var model = await _paymentService.UpdateAccounts(vm);
       
        if(model == null)
        {
            TempData["ErrorMessage"] = "No tiene suficiente dinero para realizar este Pago.";
            return RedirectToAction("Express");

        }

        // Redirigir a la misma vista para confirmar la operación exitosa
        return RedirectToAction("Express");
    }

    // Método para obtener el propietario de una cuenta
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


    // Método GET para cargar la vista inicial con las cuentas
    public async Task<IActionResult> CreditCard()
    {
        var model = await _bankAccountService.GetAccounts();
        SavePaymentViewModel vm = new()
        {
            accounts = model
        };

        return View(vm);
    }


    // Método POST para procesar el formulario de pago
    [HttpPost]
    public async Task<IActionResult> CreditCard(SavePaymentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            // Enviar nuevamente la lista de cuentas al modelo en caso de errores de validación
            vm.accounts = await _bankAccountService.GetAccounts();
            return View(vm);  // Muestra la misma vista con errores
        }

       var model =  await _paymentService.UpdateCreditAccounts(vm);


        if (model == null)
        {
            TempData["ErrorMessage"] = "No dispone de suficiente dinero para realizar esta transacción.";
            return RedirectToAction("CreditCard");

        }

        // Redirigir a la misma vista para confirmar la operación exitosa
        return RedirectToAction("CreditCard");
    }



    public async Task<IActionResult> Loan()
    {
        var model = await _bankAccountService.GetAccounts();
        SavePaymentViewModel vm = new()
        {
            accounts = model
        };

        return View(vm);
    }




    [HttpPost]
    public async Task<IActionResult> Loan(SavePaymentViewModel vm)
    {
        if (!ModelState.IsValid)
        {
            
           
            return RedirectToAction("Loan");  
        }

        var model = await _paymentService.UpdateCreditAccounts(vm);


        if (model == null)
        {
            TempData["ErrorMessage"] = "No tiene suficiente dinero para realizar esta transacción.";
            return RedirectToAction("Loan");

        }

        // Redirigir a la misma vista para confirmar la operación exitosa
        return RedirectToAction("Loan");
    }


    public async Task<IActionResult> Beneficiaries()
    {
        var model = await _bankAccountService.GetAccounts();
        var beneficiaries = await _beneficiariesService.LoadBeneficiary();
        SavePaymentViewModel vm = new()
        {
            accounts = model,
            beneficiary = beneficiaries
        };

        return View(vm);
    }

    [HttpPost]
    public async Task<IActionResult> Beneficiaries(SavePaymentViewModel vm)
    {
        if (!ModelState.IsValid)
        {


            return RedirectToAction("Beneficiaries");
        }

        var model = await _paymentService.UpdateBeneficiary(vm);


        if (model == null)
        {
            TempData["ErrorMessage"] = "No tiene suficiente dinero para realizar esta transacción.";
            return RedirectToAction("Beneficiaries");

        }

        // Redirigir a la misma vista para confirmar la operación exitosa
        return RedirectToAction("Beneficiaries");
    }

}
