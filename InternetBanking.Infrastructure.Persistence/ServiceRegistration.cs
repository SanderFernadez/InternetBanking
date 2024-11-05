using InternetBanking.Core.Application.Interfaces.Repositories;
using InternetBanking.Infrastructure.Persistence.Contexts;
using InternetBanking.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace InternetBanking.Infrastructure.Persistence
{
    public static class ServicesRegistration
    {
        public static void AddPersistenceInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            #region Contexts

            if (configuration.GetValue<bool>("UseInMemoryDatabase"))
            {
                services.AddDbContext<PersistenceContext>(options => options.UseInMemoryDatabase("AppilicationDb"));
            }

            else
            {

                services.AddDbContext<PersistenceContext>(opt =>
                opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection"), m => m.MigrationsAssembly(typeof(PersistenceContext).Assembly.FullName)));
            }
            #endregion

           

            #region Repositories
            services.AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddTransient<IBankAccountRepository, BankAccountRepository>();
            services.AddTransient<ITransactionRepository, TransactionRepository>();
            services.AddTransient<IAdvanceRepository, AdvanceRepository>();
            services.AddTransient<IBeneficiaryRepository, BeneficiaryRepository>();
            services.AddTransient<IPaymentRepository, PaymentRepository>();
            services.AddTransient<ITransferRepository, TransferRepository>();

            #endregion

        }
    }
}
