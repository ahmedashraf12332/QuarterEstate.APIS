

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Quarter.Core;
using Quarter.Core.Mapping.Estates;
using Quarter.Core.ServiceContract;
using Quarter.Repostory;
using Quarter.Repostory.Data.Context;
using Quarter.Service.Service.Estates;
using QuarterEstate.APIS.Errors;

namespace Store.APIS.Helper
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddControllers();
            services.AddSwaggerService();
            services.AddDbContextService(configuration);
            services.AddAutoMapperService(configuration);
            services.AddUserDefinedService();
            services.AddInvalidModelResponseService();

            return services;
        }

        private static IServiceCollection AddBuiltInService(this IServiceCollection services)
        {
            services.AddControllers();
            return services;
        }

        private static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen();

            return services;
        }

        private static IServiceCollection AddDbContextService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<QuarterDbContexts>(option =>
            {
                option.UseSqlServer(configuration.GetConnectionString("DefaultConnection12"));
            });

            return services;
        }

        private static IServiceCollection AddUserDefinedService(this IServiceCollection services)
        {
            services.AddScoped<IProductService, EstateService>();
            services.AddScoped<IUnitofWork, UnitOfWork>();

            return services;
        }

        private static IServiceCollection AddAutoMapperService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAutoMapper(M => M.AddProfile(new EstateProfile(configuration)));

            return services;
        }

        private static IServiceCollection AddInvalidModelResponseService(this IServiceCollection services)
        {
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = (actionContext) =>
                {
                    var errors = actionContext.ModelState?.Where(p => p.Value?.Errors.Count > 0) // Added null checks to fix CS8602
                                                        .SelectMany(p => p.Value.Errors)
                                                        .Select(e => e.ErrorMessage)
                                                        .ToArray() ?? Array.Empty<string>(); // Added fallback to empty array

                    var response = new ApiValidationErorrResponse()
                    {
                        Erorrs = errors
                    };

                    return new BadRequestObjectResult(response);
                };
            });
            return services;
        }
    }
}
