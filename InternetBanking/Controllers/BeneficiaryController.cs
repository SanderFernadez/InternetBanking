﻿using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApp.InternetBanking.Controllers

{
    [Authorize]
    public class BeneficiaryController : Controller
    {

        private readonly IBeneficiaryService _beneficiaryService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public BeneficiaryController(IBeneficiaryService beneficiaryService, IHttpContextAccessor httpContextAccessor)
        {
            _beneficiaryService = beneficiaryService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Index()
        {
            var beneficiaries = await _beneficiaryService.LoadBeneficiary(); 


            return View(beneficiaries);
        }

        [HttpPost]
        [Authorize(Roles = "Client")]
        public async Task<IActionResult> DeletePost (int Id)
        {
            await _beneficiaryService.Delete(Id);
            return RedirectToRoute(new { Controller = "Beneficiary", action = "Index" });
        }



        [Authorize(Roles = "Client")]
        public async Task<IActionResult> AddBeneficiary(int beneficiaryAccount)

        {

            var filter = await _beneficiaryService.FilterBeneficiary(beneficiaryAccount);
            
            if (!filter)
            {
                TempData["ErrorMessage"] = "Solo se pueden agregar cuentas de ahorro como beneficiarios";
                return RedirectToAction("Index");  // Redirige a la vista de tu elección  
            }

            var result = await _beneficiaryService.AddBeneficiaryAccount(beneficiaryAccount);

            if (result == null)
            {
                TempData["ErrorMessage"] = "No se pudo agregar el beneficiario. Puede ser que la cuenta no exista o ya esté registrada como beneficiario.";
                return RedirectToAction("Index");  // Redirige a la vista de tu elección
            }

            // Beneficiario agregado con éxito
            TempData["SuccessMessage"] = "Beneficiario agregado exitosamente.";
            return RedirectToAction("Index");
        }





    }
}