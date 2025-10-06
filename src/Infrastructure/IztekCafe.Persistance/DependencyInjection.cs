using IztekCafe.Persistance.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IztekCafe.Persistance
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddPersistanceServices(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlServer")!;
            services.AddDbContext<AppDbContext>(opts => opts.UseSqlServer(connectionString));

            return services;
        }
    }
}