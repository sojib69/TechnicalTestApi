using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using TechnicalTest.Api.Configurations;

namespace TechnicalTest.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ReadApplicationConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppConfiguration>(configuration.GetSection(nameof(AppConfiguration)));
            return services;
        }

        internal static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                //c.DocumentFilter<LowercaseDocumentFilter>();
                //Refer - https://gist.github.com/rafalkasa/01d5e3b265e5aa075678e0adfd54e23f

                // include all project's xml comments
                var baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (!assembly.IsDynamic)
                    {
                        var xmlFile = $"{assembly.GetName().Name}.xml";
                        var xmlPath = Path.Combine(baseDirectory, xmlFile);
                        if (File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath);
                        }
                    }
                }

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "TechicalTest.Api's",
                    License = new OpenApiLicense
                    {
                        Name = "TechicalTest",
                        Url = new Uri("https://opensource.org/licenses/MIT")
                    }
                });
                c.TagActionsBy(api =>
                {
                    if (api.GroupName != null)
                    {
                        return new[] { api.GroupName };
                    }

                    if (api.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
                    {
                        return new[] { controllerActionDescriptor.ControllerName };
                    }

                    throw new InvalidOperationException("Unable to determine tag for endpoint.");
                });
                c.AddSecurityDefinition(JwtBearerDefaults.AuthenticationScheme, new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Please enter token",
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    BearerFormat = "JWT",
                    Scheme = JwtBearerDefaults.AuthenticationScheme
                });

                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = nameof(SecuritySchemeType.OAuth2)
                        },
                        Scheme = nameof(SecuritySchemeType.OAuth2),
                        Name = nameof(SecuritySchemeType.OAuth2),
                        In = ParameterLocation.Header
                    }] = Array.Empty<string>(),

                    [new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = JwtBearerDefaults.AuthenticationScheme
                        }
                    }] = Array.Empty<string>()

                });
                c.DocInclusionPredicate((name, api) => true);
            });
            return services;
        }

        public static IServiceCollection AddCustomCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                using ServiceProvider sp = services.BuildServiceProvider();
                AppConfiguration appConfiguration = sp.GetRequiredService<IOptionsSnapshot<AppConfiguration>>().Value;
                string feUrl = appConfiguration.CorsDomains;
                string[] originUrls = feUrl.Split(",");

                options.AddPolicy(name: appConfiguration.CorsPolicyName,
                    policy =>
                    {
                        //policy.WithOrigins(originUrls)
                        policy.AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
            });
            return services;
        }
    }
}
