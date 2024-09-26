using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TechnicalTest.Api.Configurations;

namespace TechnicalTest.Api.Extensions
{
    internal static class ApplicationBuilderExtensions
    {
        internal static IApplicationBuilder UseExceptionHandling(
            this IApplicationBuilder app,
            IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            return app;
        }

        internal static void ConfigureSwagger(this IApplicationBuilder app, IConfiguration configuration)
        {
            app.UseSwagger(options =>
            {
                string? basePath = configuration["AppSettings:HostBasePath"];
                if (!string.IsNullOrEmpty(basePath))
                {
                    options.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        swaggerDoc.Servers = new List<OpenApiServer> { new OpenApiServer { Url = $"{httpReq.Scheme}://{httpReq.Host.Value}{basePath}" } };
                    });
                }
            });

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("v1/swagger.json", typeof(Program).Assembly.GetName().Name);
                options.DisplayRequestDuration();

                using (IServiceScope scope = app.ApplicationServices.CreateScope())
                {
                    AzureAdB2cSetting azureAdSetting = scope.ServiceProvider.GetRequiredService<IOptionsSnapshot<AzureAdB2cSetting>>().Value;
                    options.OAuthClientId(azureAdSetting.ClientId);
                    options.OAuthClientSecret(azureAdSetting.ClientSecret);
                }
                options.OAuthUseBasicAuthenticationWithAccessCodeGrant();
            });
        }

        internal static IApplicationBuilder UseEndpoints(this IApplicationBuilder app)
            => app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
    }
}
