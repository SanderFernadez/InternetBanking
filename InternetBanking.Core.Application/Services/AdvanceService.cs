
using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Advances;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class AdvanceService: GenericService<SaveAdvanceViewModel, AdvanceViewModel, Advance>, IAdvanceService
    {
        private readonly IAdvanceRepository advanceRepository;
        private readonly IMapper _mapper;

        public AdvanceService(IAdvanceRepository advanceRepository, IMapper mapper): base(advanceRepository,mapper)
        {
            this.advanceRepository = advanceRepository;
            _mapper = mapper;
        }
    }
}
