using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;
using InternetBanking.Core.Application.ViewModels.Payments;

public class PaymentsController : Controller
{
    public IActionResult Express()
    {
        var model = new SavePaymentViewModel();

        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult ProcesarExpress(SavePaymentViewModel model)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Confirmacion");
        }

        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View("Express", model);
    }

    public IActionResult CreditCard()
    {
        var model = new SavePaymentViewModel();

        ViewBag.TarjetasCredito = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Tarjeta Visa - 1234" },
            new SelectListItem { Value = "2", Text = "Tarjeta Mastercard - 5678" }
        };
        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult ProcesarCreditCard(SavePaymentViewModel model)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Confirmacion");
        }

        ViewBag.TarjetasCredito = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Tarjeta Visa - 1234" },
            new SelectListItem { Value = "2", Text = "Tarjeta Mastercard - 5678" }
        };
        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View("CreditCard", model);
    }

    public IActionResult Loan()
    {
        var model = new SavePaymentViewModel();

        ViewBag.Prestamos = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Préstamo Personal - 1000" },
            new SelectListItem { Value = "2", Text = "Préstamo Hipotecario - 2000" }
        };
        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult ProcesarLoan(SavePaymentViewModel model)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Confirmacion");
        }

        ViewBag.Prestamos = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Préstamo Personal - 1000" },
            new SelectListItem { Value = "2", Text = "Préstamo Hipotecario - 2000" }
        };
        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View("Loan", model);
    }

    public IActionResult Beneficiaries()
    {
        var model = new SavePaymentViewModel();

        ViewBag.Beneficiarios = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Juan Pérez" },
            new SelectListItem { Value = "2", Text = "Ana Gómez" }
        };
        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult ProcesarBeneficiaries(SavePaymentViewModel model)
    {
        if (ModelState.IsValid)
        {
            return RedirectToAction("Confirmacion");
        }

        ViewBag.Beneficiarios = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Juan Pérez" },
            new SelectListItem { Value = "2", Text = "Ana Gómez" }
        };
        ViewBag.CuentasOrigen = new List<SelectListItem>
        {
            new SelectListItem { Value = "1", Text = "Cuenta Ahorro - 123456" },
            new SelectListItem { Value = "2", Text = "Cuenta Corriente - 789012" }
        };

        return View("Beneficiaries", model);
    }

    public IActionResult Confirmacion()
    {
        return View();
    }
}
