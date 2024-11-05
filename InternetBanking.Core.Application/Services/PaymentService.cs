

using AutoMapper;
using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.ViewModels.Payments;
using InternetBanking.Core.Domain.Entities;

namespace InternetBanking.Core.Application.Services
{
    public class PaymentService : GenericService<SavePaymentViewModel, PaymentViewModel, Payment>, IPaymentService 

    {
        private readonly IPaymentRepository _paymentRepository;
        private readonly IMapper _mapper;

        public PaymentService(IPaymentRepository paymentRepository, IMapper mapper) : base( paymentRepository,mapper) 
        {
            _paymentRepository = paymentRepository;
            _mapper = mapper;
        }






    }
}
