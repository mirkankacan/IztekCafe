using IztekCafe.Application.Dtos.StockDtos;
using IztekCafe.Domain.Entities;
using Mapster;

namespace IztekCafe.Application.MappingConfigs
{
    public class StockMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Stock, StockDto>()
                .Map(dest => dest.ProductName, src => src.Product.Name);

            config.NewConfig<Stock, StockDetailDto>()
                   .Map(dest => dest.ProductName, src => src.Product.Name);
        }
    }
}