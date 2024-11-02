using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Core.Application.Services;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Core.Application
{
    public static class ServiceRegistration
    {


          public static void AddApplicationLayer(this IServiceCollection services)
         {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());

            #region Services
            /* services.AddTransient(typeof(IGenericService<,,>), typeof(GenericService<,,>));*/
            services.AddTransient<IUserService, UserService>();

            #endregion

        }

        
    }
}
