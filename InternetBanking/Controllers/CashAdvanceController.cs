using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.ViewModels.Advance;
using System.Collections.Generic;
using System.Linq;

public class CashAdvanceController : Controller
{
    // Simulación de datos en memoria para tarjetas de crédito y cuentas de ahorro
    private static List<AdvanceViewModel> advances = new List<AdvanceViewModel>();

    private static List<AdvanceViewModel> creditCards = new List<AdvanceViewModel>
    {
        new AdvanceViewModel { AccountCreditId = 1, AccountCreditName = "Tarjeta Visa", Amount = 500 },
        new AdvanceViewModel { AccountCreditId = 2, AccountCreditName = "Tarjeta Mastercard", Amount = 1000 }
    };

    private static List<AdvanceViewModel> savingAccounts = new List<AdvanceViewModel>
    {
        new AdvanceViewModel { DestinationAccountId = 1, DestinationAccountName = "Cuenta de Ahorro - 123456" },
        new AdvanceViewModel { DestinationAccountId = 2, DestinationAccountName = "Cuenta de Ahorro - 654321" }
    };

    public IActionResult Index()
    {
        var model = new AdvanceViewModel();
        ViewBag.CreditCards = creditCards;
        ViewBag.SavingAccounts = savingAccounts;

        return View(model);
    }

    [HttpPost]
    public IActionResult RealizarAvance(AdvanceViewModel model)
    {
        // Validar los datos
        if (model.Amount <= 0 || model.AccountCreditId == 0 || model.DestinationAccountId == 0)
        {
            TempData["Error"] = "Todos los campos son requeridos.";
            return RedirectToAction("Index");
        }

        // Obtener la tarjeta de crédito seleccionada
        var selectedCard = creditCards.FirstOrDefault(c => c.AccountCreditId == model.AccountCreditId);
        if (selectedCard == null)
        {
            TempData["Error"] = "Tarjeta de crédito no encontrada.";
            return RedirectToAction("Index");
        }

        // Verificar si el monto del avance supera el límite de crédito
        if (model.Amount > selectedCard.Amount)
        {
            TempData["Error"] = "El monto del avance no puede exceder el límite de crédito de la tarjeta.";
            return RedirectToAction("Index");
        }

        // Obtener la cuenta de ahorro seleccionada
        var selectedAccount = savingAccounts.FirstOrDefault(a => a.DestinationAccountId == model.DestinationAccountId);
        if (selectedAccount == null)
        {
            TempData["Error"] = "Cuenta de ahorro no encontrada.";
            return RedirectToAction("Index");
        }

        // Realizar el avance de efectivo
        // Aquí podrías agregar lógica para actualizar la base de datos con el avance.
        advances.Add(new AdvanceViewModel
        {
            Amount = model.Amount,
            AccountCreditId = model.AccountCreditId,
            DestinationAccountId = model.DestinationAccountId,
            DateAdvance = DateTime.Now
        });

        TempData["Success"] = "Avance de efectivo realizado correctamente.";
        return RedirectToAction("Index");
    }
}
