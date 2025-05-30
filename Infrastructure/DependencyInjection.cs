using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Repositories;
using Repositories.Interface;
using Repositories.Repositories;
using Services.Interface;
using Services.Services;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            //DbContext
            services.AddDbContext<FunewsManagementContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));


            //DI for Repositories
            services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
            services.AddScoped<IUnitOfWork, UnitOfWork>();


            //DI for Services
            services.AddScoped<ISystemAccountService, SystemAccountService>();

            return services;
        }
    }
}
