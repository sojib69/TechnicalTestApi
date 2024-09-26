using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System.Reflection;
using TechnicalTest.Api.Configurations;

namespace TechnicalTest.Api.Extensions
{
    internal static class ServiceCollectionExtensions
    {
        internal static IServiceCollection ReadApplicationConfigurations(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<AppConfiguration>(configuration.GetSection(nameof(AppConfiguration)))
                .Configure<AzureAdB2cSetting>(configuration.GetSection("AzureAdB2c"));
            return services;
        }

        internal static IServiceCollection RegisterSwagger(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                // include all project's xml comments
                string baseDirectory = AppDomain.CurrentDomain.BaseDirectory;

                foreach (Assembly assembly in AppDomain.CurrentDomain.GetAssemblies())
                {
                    if (!assembly.IsDynamic)
                    {
                        string xmlFile = $"{assembly.GetName().Name}.xml";
                        string xmlPath = Path.Combine(baseDirectory, xmlFile);
                        if (File.Exists(xmlPath))
                        {
                            c.IncludeXmlComments(xmlPath);
                        }
                    }
                }

                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Technical Test API(s)",
                    License = new OpenApiLicense
                    {
                        Name = "Technical Test",
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

                using (ServiceProvider sp = services.BuildServiceProvider())
                {
                    AzureAdB2cSetting azureAdSetting = sp.GetRequiredService<IOptionsSnapshot<AzureAdB2cSetting>>().Value;
                    string signUpSignInPolicyIdSegment = $"/{azureAdSetting.SignUpSignInPolicyId}";
                    c.AddSecurityDefinition(nameof(SecuritySchemeType.OAuth2), new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.OAuth2,
                        Flows = new OpenApiOAuthFlows
                        {
                            Implicit = new OpenApiOAuthFlow()
                            {
                                AuthorizationUrl = new Uri(new Uri(azureAdSetting.Instance), $"{azureAdSetting.Domain}{signUpSignInPolicyIdSegment}/oauth2/v2.0/authorize"),
                                TokenUrl = new Uri(new Uri(azureAdSetting.Instance), $"{azureAdSetting.Domain}{signUpSignInPolicyIdSegment}/oauth2/v2.0/token"),
                                Scopes = azureAdSetting.Scopes!.Split(' ').ToDictionary(k => k, k => "Access application on user behalf")
                            }
                        }
                    });
                }

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
                string feUrl = appConfiguration.CorsDomains ?? "";
                string[] originUrls = feUrl.Split(",");

                options.AddPolicy(name: appConfiguration.CorsPolicyName ?? "",
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
