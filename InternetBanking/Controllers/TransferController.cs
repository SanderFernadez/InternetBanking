using Microsoft.AspNetCore.Mvc;
using InternetBanking.Core.Application.ViewModels.Transfers;
using System.Collections.Generic;
using System.Linq;

public class TransferController : Controller
{
    // Simulación de datos de cuentas
    private static List<TransferViewModel> accounts = new List<TransferViewModel>
    {
        new TransferViewModel { Id = 1, AccountSourceId = 1, DestinationAccountId = 2, Amount = 100 },
        new TransferViewModel { Id = 2, AccountSourceId = 2, DestinationAccountId = 1, Amount = 200 }
    };

    public IActionResult Index()
    {
        var model = new SaveTransferViewModel();
        ViewBag.Accounts = accounts; // Pasa las cuentas a la vista
        return View(model);
    }

    [HttpPost]
    public IActionResult RealizarTransferencia(SaveTransferViewModel model)
    {
        // Validar los datos
        if (model.Amount <= 0 || model.AccountSourceId == 0 || model.DestinationAccountId == 0)
        {
            TempData["Error"] = "Todos los campos son requeridos.";
            return RedirectToAction("Index");
        }

        var sourceAccount = accounts.FirstOrDefault(a => a.AccountSourceId == model.AccountSourceId);
        var destinationAccount = accounts.FirstOrDefault(a => a.DestinationAccountId == model.DestinationAccountId);

        if (sourceAccount == null || destinationAccount == null)
        {
            TempData["Error"] = "Las cuentas seleccionadas no son válidas.";
            return RedirectToAction("Index");
        }

        if (model.Amount > sourceAccount.Amount)
        {
            TempData["Error"] = "No tienes suficiente saldo en la cuenta de origen.";
            return RedirectToAction("Index");
        }

        // Realizar la transferencia
        sourceAccount.Amount -= model.Amount; // Resta del saldo de la cuenta de origen
        destinationAccount.Amount += model.Amount; // Agrega al saldo de la cuenta de destino

        TempData["Success"] = "Transferencia realizada correctamente.";
        return RedirectToAction("Index");
    }
}
