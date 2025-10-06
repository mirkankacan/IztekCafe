using FluentValidation;
using Mapster;
using Microsoft.Extensions.DependencyInjection;

namespace IztekCafe.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMapster();
            services.AddValidatorsFromAssembly(typeof(ApplicationAssembly).Assembly);
            var config = TypeAdapterConfig.GlobalSettings;
            config.Scan(typeof(ApplicationAssembly).Assembly);
            return services;
        }
    }
}