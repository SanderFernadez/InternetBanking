using InternetBanking.Core.Application.Interfaces.Services;
using InternetBanking.Infrastructure.Identity.Contexts;
using InternetBanking.Infrastructure.Identity.Entities;
using InternetBanking.Infrastructure.Identity.Seeds;
using InternetBanking.Infrastructure.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InternetBanking.Infrastructure.Identity
{
    public static class ServiceRegistration
    {
        public static void AddIdentityLayer(this IServiceCollection services, IConfiguration configuration)
        {

            #region Contexts

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<IdentityContext>(options => options.UseInMemoryDatabase("IdentityDbs"));
            }

            else
            {

                services.AddDbContext<IdentityContext>(opt =>
                {
                    opt.EnableSensitiveDataLogging();
                    opt.UseSqlServer(configuration.GetConnectionString("IdentityConnection"),
                        m => m.MigrationsAssembly(typeof(IdentityContext).Assembly.FullName));

                });

            }
            #endregion

            #region Identity

            services.AddIdentityCore<ApplicationUser>()
                    .AddRoles<IdentityRole>()
                    .AddSignInManager()
                    .AddEntityFrameworkStores<IdentityContext>()
                    .AddTokenProvider<DataProtectorTokenProvider<ApplicationUser>>(TokenOptions.DefaultProvider);

            services.Configure<DataProtectionTokenProviderOptions>(opt =>
            {
                opt.TokenLifespan = TimeSpan.FromSeconds(300);
            });

            services.AddAuthentication(opt =>
            {
                opt.DefaultScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultChallengeScheme = IdentityConstants.ApplicationScheme;
                opt.DefaultSignInScheme = IdentityConstants.ApplicationScheme;
            }).AddCookie(IdentityConstants.ApplicationScheme, opt =>
            {
                opt.ExpireTimeSpan = TimeSpan.FromHours(24);
                opt.LoginPath = "/User";
                opt.AccessDeniedPath = "/User/AccessDenied"; //por crear 
            });
            #endregion

            #region Services
            services.AddTransient<IAccountService, AccountService>();
            #endregion


        }





        #region "Seeds"
        public static async Task RunSeedsAsysc(this IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var services = scope.ServiceProvider;

                try
                {
                    var userManager = services.GetRequiredService<UserManager<ApplicationUser>>();
                    var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();

                    await DefaultRoles.SeedAsync(roleManager);
                    await DefaultSuperAdminUser.SeedAsync(userManager);
                    await DefaultClientUser.SeedAsync(userManager);
                    await DefaultAdminUser.SeedAsync(userManager);
                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion

    }
}
