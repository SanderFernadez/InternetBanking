using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using InternetBanking.Core.Application.Enums;
using InternetBanking.Core.Application.ViewModels.BankAccounts;
using InternetBanking.Core.Application.ViewModels.Beneficiary;

namespace InternetBanking.Core.Application.ViewModels.Payments
{
    public class SavePaymentViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La cuenta de origen es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una cuenta de origen válida.")]
        public int SourceAccount { get; set; }

        [Required(ErrorMessage = "La cuenta de destino es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "Seleccione una cuenta de destino válida.")]
        public int DestinationAccount { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El monto debe ser mayor que cero.")]
        public decimal AmountPaid { get; set; }

       
        public DateTime PaymentDate { get; set; }

      
        public TransferType TransactionType { get; set; }

        public ICollection<BankAccountViewModel> accounts { get; set; } = new List<BankAccountViewModel>();
        public ICollection<SaveBeneficiaryViewModel> beneficiary { get; set; } = new List<SaveBeneficiaryViewModel>();
    }
}
