
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Writers;
using Quarter.Core;
using Quarter.Core.Mapping.Estates;
using Quarter.Core.ServiceContract;
using Quarter.Repostory;
using Quarter.Repostory.Data.Context;
using Quarter.Service.Service.Estates;

namespace QuarterEstate.APIS
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddDbContext<QuarterDbContexts>(option => {
            
                option.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection12"));
            });
            builder.Services.AddScoped<IProductService,EstateService>();
            builder.Services.AddScoped<IUnitofWork,UnitofWork>();
            builder.Services.AddAutoMapper(M => M.AddProfile(new EstateProfile()));

            var app = builder.Build();
            using var scope = app.Services.CreateScope();

           
            var service = scope.ServiceProvider;
            var context = service.GetRequiredService<QuarterDbContexts>();
            var LoggerFactory = service.GetRequiredService<ILoggerFactory>();
            try
            {
                context.Database.MigrateAsync();
                await StoreDbContextSeed.SeedAsync(context);
            }
            catch (Exception ex)
            {
                var logger = LoggerFactory.CreateLogger<Program>();
                logger.LogError(ex, "›Ì „‘«ﬂ· Ì«’Õ»Ì");
            }
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
                {
                    app.UseSwagger();
                    app.UseSwaggerUI();
                }

                app.UseHttpsRedirection();

                app.UseAuthorization();


                app.MapControllers();

                app.Run();
            }
    }
    }

