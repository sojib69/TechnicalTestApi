using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Web;
using TechnicalTest.Api.Configurations;
using TechnicalTest.Api.Extensions;
using TechnicalTest.Application.Extensions;
using TechnicalTest.Infrastructure.Contexts;
using TechnicalTest.Infrastructure.Extensions;

namespace TechnicalTest.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // TODO: Need to uncommented this for authentication
            //builder.Services.AddAuthentication()
            //    .AddMicrosoftIdentityWebApi(configuration.GetSection("AzureAdB2c"))
            //    .EnableTokenAcquisitionToCallDownstreamApi()
            //    .AddInMemoryTokenCaches();

            // Database
            builder.Services.AddDbContext<AppDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            builder.Services.ReadApplicationConfigurations(configuration);

            builder.Services.AddSwaggerGen(); // TODO: Need to remove this for authentication
            //builder.Services.RegisterSwagger(); // TODO: Need to add this for authentication

            builder.Services.AddCustomCors();

            // Services added from Application and Infrastructure layer
            builder.Services.AddApplicationLayer();
            builder.Services.AddInfrastructure();

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddFluentValidationAutoValidation();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            //app.ConfigureSwagger(configuration); // TODO: Need to uncommented this for authentication
            app.UseCors(configuration.GetSection(nameof(AppConfiguration)).GetValue<string>("CorsPolicyName") ?? string.Empty);
            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            // Database migration
            using (var serviceScope = ((IApplicationBuilder)app).ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<AppDbContext>();
                context?.Database.Migrate();
            }

            app.Run();
        }
    }
}
