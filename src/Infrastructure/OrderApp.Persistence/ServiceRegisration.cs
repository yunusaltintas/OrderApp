using OrderApp.Application.Interfaces.IRepository;
using OrderApp.Application.Interfaces.IUnitOfWork;
using OrderApp.Persistence.Context;
using OrderApp.Persistence.Repository;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderApp.Persistence
{
    public static class ServiceRegisration
    {
        public static void AddPersistanceServices(this IServiceCollection services, string connectionString)
        {
            services.AddDbContextPool<ECommerceDbContext>(options => options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

            services.AddScoped<IUnitOfWork, OrderApp.Persistence.UnitOfWork.UnitOfWork>();
            services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
        }

        public static void DatabaseInitialize(this IApplicationBuilder builder)
        {

            using var serviceScope =
                builder.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();

            using var context = serviceScope.ServiceProvider.GetService<ECommerceDbContext>();

            if (context == null) return;
            DatabaseMigration(context);

            context.SaveChanges();
        }

        private static void DatabaseMigration(ECommerceDbContext context)
        {
            context.Database.Migrate();
        }


    }
}
