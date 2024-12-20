﻿
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Interfaces.Services
{
    public interface IBeneficiaryService: IGenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>
    {
        Task<List<BeneficiaryViewModel>> LoadBeneficiary();
        Task<BeneficiaryViewModel> AddBeneficiaryAccount(int beneficiaryAccount);
        Task<bool> FilterBeneficiary(int accountnumber); 
    }
}
