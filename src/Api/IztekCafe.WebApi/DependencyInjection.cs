using Carter;

namespace IztekCafe.WebApi
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddWebApiServices(this IServiceCollection services)
        {
            services.AddCarter();
            services.AddOpenApi();
            services.AddEndpointsApiExplorer();
            services.AddSwaggerGen(opts =>
            {
                opts.SwaggerDoc("v1", new()
                {
                    Title = "IztekCafe API",
                    Version = "v1"
                });
            });
            return services;
        }
    }
}