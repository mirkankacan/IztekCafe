using IztekCafe.Application.Contracts.Repositories;
using IztekCafe.Application.Contracts.Services;
using IztekCafe.Application.Contracts.UnitOfWork;
using IztekCafe.Persistance.Data.Context;
using IztekCafe.Persistance.Repositories;
using IztekCafe.Persistance.Services;
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
            services.AddScoped<IUnitOfWork, UnitOfWork.UnitOfWork>();
            services.AddScoped(typeof(IGenericRepository<,>), typeof(GenericRepository<,>));
            services.AddScoped<IStockRepository, StockRepository>();
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IOrderItemRepository, OrderItemRepository>();
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ITableRepository, TableRepository>();
            services.AddScoped<IPaymentRepository, PaymentRepository>();

            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IStockService, StockService>();
            services.AddScoped<IPaymentService, PaymentService>();
            return services;
        }
    }
}