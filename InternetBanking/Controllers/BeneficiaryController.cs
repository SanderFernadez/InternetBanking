using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using InternetBanking.Core.Application.ViewModels.Beneficiary;

public class BeneficiariesController : Controller
{
    // Simulación de base de datos en memoria
    private static List<BeneficiaryViewModel> beneficiaries = new List<BeneficiaryViewModel>();

    // Método para mostrar la lista de beneficiarios
    public IActionResult Index()
    {
        return View(beneficiaries);
    }

    // Método para agregar un beneficiario
    [HttpPost]
    public IActionResult AddBeneficiary(SaveBeneficiaryViewModel model)
    {
        // Validar que el número de cuenta no esté vacío
        if (model.BeneficiaryAccount <= 0)
        {
            TempData["Error"] = "El número de cuenta es requerido.";
            return RedirectToAction("Index");
        }

        // Simulación de verificación de cuenta (reemplazar con la lógica real)
        bool accountExists = model.BeneficiaryAccount == 123456789; // Cambia esto por la verificación real

        if (!accountExists)
        {
            TempData["Error"] = "El número de cuenta no existe.";
            return RedirectToAction("Index");
        }

        // Crear un nuevo beneficiario y agregarlo a la lista
        var newBeneficiary = new BeneficiaryViewModel
        {
            Id = beneficiaries.Count + 1,
            UserId = model.UserId, // Asegúrate de que el UserId esté configurado correctamente
            FirstName = model.FirstName,
            LastName = model.LastName,
            BeneficiaryAccount = model.BeneficiaryAccount
        };
        beneficiaries.Add(newBeneficiary);

        TempData["Success"] = "Beneficiario agregado correctamente.";
        return RedirectToAction("Index");
    }

    // Método para eliminar un beneficiario
    public IActionResult DeleteBeneficiary(int id)
    {
        // Buscar el beneficiario por ID
        var beneficiary = beneficiaries.FirstOrDefault(b => b.Id == id);
        if (beneficiary != null)
        {
            beneficiaries.Remove(beneficiary);
            TempData["Success"] = "Beneficiario eliminado correctamente.";
        }
        else
        {
            TempData["Error"] = "Beneficiario no encontrado.";
        }
        return RedirectToAction("Index");
    }
}
