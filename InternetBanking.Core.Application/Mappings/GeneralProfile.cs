using AutoMapper;
using InternetBanking.Core.Application.Dtos.Account;
using InternetBanking.Core.Application.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application.Mappings
{
    public class GeneralProfile : Profile

    { 
        public GeneralProfile()
    {


            #region

            CreateMap<SaveUserViewModel, AuthenticationResponse>()

                .ReverseMap()               
                .ForMember(x => x.ConfirmPassword, opt => opt.Ignore());

            CreateMap<AuthenticationRequest, LoginViewModel>()
                .ForMember(x => x.HasError, opt => opt.Ignore())
                .ForMember(x => x.Error, opt => opt.Ignore())
                .ReverseMap();


            CreateMap<RegisterRequest, SaveUserViewModel>()
               .ForMember(x => x.HasError, opt => opt.Ignore())
               .ForMember(x => x.Error, opt => opt.Ignore())
               .ReverseMap();

            #endregion
        }

    }
}
