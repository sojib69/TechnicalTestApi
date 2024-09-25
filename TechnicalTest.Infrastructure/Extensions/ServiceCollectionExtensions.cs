using LazyCache;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using TechnicalTest.Application.Interfaces.Common;
using TechnicalTest.Application.Interfaces.ContactGroups;
using TechnicalTest.Application.Interfaces.Contacts;
using TechnicalTest.Infrastructure.Repositories.Common;
using TechnicalTest.Infrastructure.Repositories.ContactGroups;
using TechnicalTest.Infrastructure.Repositories.Contacts;

namespace TechnicalTest.Infrastructure.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            return services.AddAutoMapper(Assembly.GetExecutingAssembly())
                    .AddRepositories();
        }

        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            return services
                .AddScoped(typeof(IRepositoryAsync<>), typeof(RepositoryAsync<>))
                .AddSingleton<IAppCache, CachingService>()
                .AddScoped<IUnitOfWork, UnitOfWork>()

                //Flow
                .AddScoped<IContactRepository, ContactRepository>()
                .AddScoped<IContactGroupRepository, ContactGroupRepository>();
        }
    }
}
