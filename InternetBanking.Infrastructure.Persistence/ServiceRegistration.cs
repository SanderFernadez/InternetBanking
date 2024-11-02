using InternetBanking.Infrastructure.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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




            //services.AddScoped<IPostRepository, PostRepository>();
            //services.AddScoped<ICommentRepository, CommentRepository>();
            //services.AddScoped<IFriendshipRepository, FriendshipRepository>();

            #endregion

        }
    }
}
