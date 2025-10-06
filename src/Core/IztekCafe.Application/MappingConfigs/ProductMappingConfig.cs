using IztekCafe.Application.Dtos.ProductDtos;
using IztekCafe.Domain.Entities;
using IztekCafe.Domain.Extensions;
using Mapster;

namespace IztekCafe.Application.MappingConfigs
{
    public class ProductMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<CreateProductDto, Product>()
                 .Ignore(dest => dest.Category)
                 .Ignore(dest => dest.OrderItems)
                 .Ignore(dest => dest.Stock);

            config.NewConfig<Product, ProductDto>()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.HasStock, src => src.Stock != null ? src.Stock.HasStock : false)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());

            config.NewConfig<Product, ProductDetailDto>()
                .Map(dest => dest.CategoryName, src => src.Category.Name)
                .Map(dest => dest.StockQuantity, src => src.Stock != null ? src.Stock.Quantity : 0)
                .Map(dest => dest.StockUnit, src => src.Stock != null ? src.Stock.Unit : null)
                .Map(dest => dest.HasStock, src => src.Stock != null ? src.Stock.HasStock : false)
                .Map(dest => dest.StatusName, src => src.Status.ToDisplayString());
        }
    }
}