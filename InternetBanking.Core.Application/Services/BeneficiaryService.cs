
using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Beneficiary;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
       public class BeneficiaryService : GenericService<SaveBeneficiaryViewModel, BeneficiaryViewModel, Beneficiary>, IBeneficiaryService
        {
            private readonly IBeneficiaryRepository _beneficiaryRepository;
            private readonly IMapper _mapper;

            public BeneficiaryService(IBeneficiaryRepository beneficiaryRepository, IMapper mapper) : base(beneficiaryRepository, mapper)
            {
                 _beneficiaryRepository = beneficiaryRepository;
                _mapper = mapper;
            }
        }
    
}
