using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Users;


namespace InternetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile

    { 
        public GeneralProfile()
    {


            #region user

            CreateMap<SaveUserViewModel, AuthenticationResponse>()

                .ReverseMap()               
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore());

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<AuthenticationResponse, RegisterRequest>()
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(x => x.IsVerified, opt => opt.Ignore())
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ForMember(x => x.Id, opt => opt.Ignore());



            CreateMap<RegisterRequest, SaveUserViewModel>()
               .ForMember(x => x.HasError, opt => opt.Ignore())
               .ForMember(x => x.Error, opt => opt.Ignore())
               .ReverseMap();

            #endregion
        }

    }
}
