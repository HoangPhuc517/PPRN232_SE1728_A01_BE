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
using Services.DTOs;
using Services.Interface;
using Services.Mapping;
using Services.Services;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureDI(this IServiceCollection services, IConfiguration configuration)
        {
            //DbContext
            services.AddDbContext<FunewsManagementContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

			//DI for AutoMapper
			services.AddAutoMapper(typeof(MappingProfile));

			//DI for Repositories
			services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
			services.AddScoped<IUnitOfWork, UnitOfWork>();


			//DI for Services
			services.AddScoped<INewsArticleService, NewArticleService>();
			services.AddScoped<ISystemAccountService, SystemAccountService>();
			services.AddScoped<ICategoryService, CategoryService>();

            return services;
        }
    }
}
